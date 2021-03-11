using System;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.IService.PersonManage;
using BSFramework.Data.Repository;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using BSFramework.Data;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;

namespace ERCHTMS.Service.PersonManage
{
    /// <summary>
    /// 描 述：转岗信息表
    /// </summary>
    public class TransferService : RepositoryFactory<TransferEntity>, TransferIService
    {
        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public IEnumerable<TransferEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TransferEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        public TransferEntity GetUsertraEntity(string keyValue)
        {
            string sql =
                string.Format("select * from bis_transfer where IsConfirm=1 and userid='{0}' order by CreateDate desc",
                    keyValue);
            TransferEntity tr = this.BaseRepository().FindList(sql).FirstOrDefault();

            Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
            string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", tr.OutDeptId);
            DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
            bool flag = true;
            while (dept != null && flag)
            {
                if (tr.OutDeptName == null || tr.OutDeptName == "")
                {
                    tr.OutDeptName = dept.FullName;
                }
                else
                {
                    if (dept.FullName != tr.OutDeptName)//班组同步过来的信息会本身会有名称 会出现xx|xx重复数据
                    {
                        tr.OutDeptName = dept.FullName + "|" + tr.OutDeptName;
                    }
                }

                if (dept.Nature == "班组" || dept.Nature == "专业")
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

                Repository<DepartmentEntity> Newdeptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                string newsql = string.Format("select * from base_Department  where departmentid ='{0}' ", dept.ParentId);
                dept = Newdeptdb.FindList(newsql).FirstOrDefault();
            }

            return tr;
        }

        /// <summary>
        /// 根据当前部门id获取层级显示部门
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public string GetDeptName(string deptid)
        {
            try
            {
                Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", deptid);
                DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
                string deptname = "";
                bool flag = true;
                while (dept != null && flag)
                {
                    if (deptname == "")
                    {
                        deptname = dept.FullName;
                    }
                    else
                    {
                        deptname = dept.FullName + "|" + deptname;
                    }

                    if (dept.Nature == "班组" || dept.Nature == "专业")
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }

                    Repository<DepartmentEntity> Newdeptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                    string newsql = string.Format("select * from base_Department  where departmentid ='{0}' ",
                        dept.ParentId);
                    dept = Newdeptdb.FindList(newsql).FirstOrDefault();
                }

                return deptname;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 获取当前用户所有转岗代办事项
        /// </summary>
        /// <returns></returns>
        public int GetTransferNum()
        {
            try
            {
                //获取到当前登录用户
                Operator user = OperatorProvider.Provider.Current();
                string sql = "select count(TID) from bis_transfer ";
                string where = "";

                string[] role = user.RoleName.Split(',');
                //首先判断是否是班组\专业\部门负责人
                if (role.Contains("负责人") || role.Contains("公司领导") || role.Contains("厂级部门用户"))
                {
                    //如果是负责人及以上 则查询本部门及所有下属部门的转岗数据
                    where = string.Format("where IsConfirm=1  and outdeptcode like '{0}%'", user.DeptCode);
                }
                else
                {
                    //如果是普通员工只能查看下级部门的转岗申请
                    where = string.Format("where IsConfirm=1  and outdeptcode like '{0}%' and outdeptcode != '{0}'", user.DeptCode);
                    //Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                    //string deptsql = string.Format("select * from base_Department  where parentid ='{0}' ", user.DeptId);
                    //DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
                    //if (dept != null)
                    //{
                    //    //如果是普通员工只能查看下级部门的转岗申请
                    //    where = string.Format("where outdeptcode like '{0}%'", dept.DeptCode);
                    //}
                    //else
                    //{
                    //    //如果没有下级部门则返回0;
                    //    return 0;
                    //}
                }

                return Convert.ToInt32(BaseRepository().FindObject(sql + where));
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 获取代办列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表</returns>
        public DataTable GetTransferList(Pagination pagination, string queryJson)
        {
            try
            {
                DatabaseType dataType = DbHelper.DbType;
                var queryParam = queryJson.ToJObject();
                //查询条件
                if (!queryParam["Name"].IsEmpty())//危害因素
                {
                    string Name = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and UserName  like '%{0}%'", Name.Trim());
                }

                //获取到当前登录用户
                Operator user = OperatorProvider.Provider.Current();
                string[] role = user.RoleName.Split(',');
                //首先判断是否是班组\专业\部门负责人
                if (role.Contains("负责人") || role.Contains("公司领导") || role.Contains("厂级部门用户"))
                {
                    //如果是负责人及以上 则查询本部门及所有下属部门的转岗数据
                    pagination.conditionJson += string.Format(" and outdeptcode like '{0}%'", user.DeptCode);
                }
                else
                {
                    //如果是普通员工只能查看下级部门的转岗申请
                    pagination.conditionJson += string.Format(" and (outdeptcode like '{0}%' and outdeptcode != '{0}')", user.DeptCode);
                }

                return this.BaseRepository().FindTableByProcPager(pagination, dataType);
            }
            catch (Exception ex)
            {

                throw ex;
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
        public void SaveForm(string keyValue, TransferEntity entity)
        {

            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<UserEntity> inlogdb = new Repository<UserEntity>(DbFactory.Base());
                string sql = string.Format("select * from base_user  where userid ='{0}' ", entity.UserId);
                UserEntity user = inlogdb.FindList(sql).FirstOrDefault();
                Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", entity.OutDeptId);
                DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();

                if (entity.IsConfirm == 0 || keyValue != "")
                {

                    if (keyValue == "")
                    {
                        entity.Create();
                        entity.OutDeptCode = dept.EnCode;
                        entity.CreateUserName = OperatorProvider.Provider.Current().UserName;
                        res.Insert<TransferEntity>(entity);
                    }
                    else
                    {
                        entity.Modify(keyValue);
                        //confirm为2的时候表示已完成
                        entity.IsConfirm = 2;
                        res.Update<TransferEntity>(entity);

                    }

                    //根据部门id获取所属机构Code
                    Repository<DepartmentEntity> organdb = new Repository<DepartmentEntity>(DbFactory.Base());
                    string organsql = string.Format("select * from base_department  where   departmentid='{0}' ",
                        dept.OrganizeId);
                    DepartmentEntity organ = organdb.FindList(organsql).FirstOrDefault();
                    user.OrganizeCode = organ.EnCode;


                    if (entity.OutJobId != null && entity.OutJobId != "")
                    {
                        string postcode = "";
                        Repository<RoleEntity> postdb = new Repository<RoleEntity>(DbFactory.Base());
                        string postsql = string.Format("select * from base_role where category=3 ");
                        IEnumerable<RoleEntity> rlist = postdb.FindList(postsql);
                        string[] Postids = entity.OutJobId.Split(',');
                        for (int i = 0; i < Postids.Length; i++)
                        {
                            RoleEntity ro = rlist.Where(it => it.RoleId == Postids[i]).FirstOrDefault();
                            if (ro != null)
                            {
                                if (postcode == "")
                                {
                                    postcode = ro.EnCode;
                                }
                                else
                                {
                                    postcode += "," + ro.EnCode;
                                }
                            }
                        }

                        user.PostCode = postcode;
                        user.PostName = entity.OutJobName;
                        user.PostId = entity.OutJobId;
                    }
                    else
                    {
                        user.PostCode = "";
                        user.PostName = "";
                        user.PostId = "";
                    }

                    //如果不需要确认 则直接修改用户的部门 岗位 职务信息
                    
                    user.DutyId = entity.OutPostId;
                    user.DutyName = entity.OutPostName;
                    user.DepartmentId = entity.OutDeptId;
                    user.DepartmentCode = dept.EnCode;

                    #region 给转岗用户赋默认角色
                    string roleName = "";
                    string roleId = "";
                    Repository<RoleEntity> roledb = new Repository<RoleEntity>(DbFactory.Base());
                    //如果选择的是厂级部门的话，角色会默认追加“厂级部门用户”
                    if (!(string.IsNullOrEmpty(user.DepartmentId) || user.DepartmentId == "undefined"))
                    {
                        Repository<DepartmentEntity> roledeptandb = new Repository<DepartmentEntity>(DbFactory.Base());
                        if (roledeptandb.FindEntity(user.DepartmentId).IsOrg == 1)
                        {
                            roleName += "厂级部门用户,";

                            
                            var expression = LinqExtensions.True<RoleEntity>();
                            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                            RoleEntity cj = roledb.IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.FullName == "厂级部门用户").FirstOrDefault();
                            if (cj != null)
                                roleId += cj.RoleId + ",";
                        }
                    }


                    var expression1 = LinqExtensions.True<RoleEntity>();
                    expression1 = expression1.And(t => t.Category == 2).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                    IEnumerable<RoleEntity> role = roledb.IQueryable(expression1).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.RoleId == user.DutyId);
                    RoleEntity roleentity = role.FirstOrDefault();
                    if (roleentity != null)
                    {
                        roleName += roleentity.RoleNames;
                        roleId += roleentity.RoleIds;
                    }
                    user.RoleId = roleId;
                    user.RoleName = roleName;
                    #endregion

                    #region 默认添加 角色、岗位、职位
                    res.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == user.UserId);
                    List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                    var currUser = OperatorProvider.Provider.Current();
                    string uid = currUser == null ? "" : currUser.UserId;
                    string uname = currUser == null ? "" : currUser.UserName;
                    //用户
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 6,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = user.UserId,
                        ObjectId = user.UserId,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                    //角色
                    string[] arr = user.RoleId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr)
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 2,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = item,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    //岗位
                    if (!string.IsNullOrEmpty(user.DutyId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 3,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = user.DutyId,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    //职位
                    if (!string.IsNullOrEmpty(user.PostId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 4,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = user.PostId,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    res.Insert<UserRelationEntity>(userRelationEntitys);
                    #endregion


                    user.OrganizeId = dept.OrganizeId;
                    user.IsTransfer = 0;
                    //转岗完成将记录加入到记录表中
                    Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
                    //找到之前没有结尾的工作记录填写结束时间
                    string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, entity.InDeptId, entity.TransferTime);
                    res.ExecuteBySql(Worksql);

                    #region 如果是班组将部门名称转换成 部门/班组
                    Repository<DepartmentEntity> deptdb1 = new Repository<DepartmentEntity>(DbFactory.Base());
                    string deptsql1 = string.Format("select * from base_Department  where departmentid ='{0}' ", entity.OutDeptId);
                    DepartmentEntity dept1 = deptdb1.FindList(deptsql1).FirstOrDefault();
                    string DeptName = "";
                    bool flag = true;
                    while (dept1 != null && flag)
                    {
                        if (DeptName == null || DeptName == "")
                        {
                            DeptName = dept1.FullName;
                        }
                        else
                        {
                            if (dept1.FullName != DeptName)//班组同步过来的信息会本身会有名称 会出现xx|xx重复数据
                            {
                                DeptName = dept1.FullName + "|" + DeptName;
                            }
                        }

                        if (dept1.Nature == "班组" || dept1.Nature == "专业")
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }

                        Repository<DepartmentEntity> Newdeptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                        string newsql = string.Format("select * from base_Department  where departmentid ='{0}' ", dept1.ParentId);
                        dept1 = Newdeptdb.FindList(newsql).FirstOrDefault();
                    }
                    #endregion
                    //新增一条新的岗位记录
                    WorkRecordEntity workEntity = new WorkRecordEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        DeptCode = user.DepartmentCode,
                        DeptId = user.DepartmentId,
                        EnterDate = Convert.ToDateTime(entity.TransferTime),
                        UserId = user.UserId,
                        UserName = user.RealName,
                        DeptName = DeptName,
                        PostName = user.DutyName,
                        CreateDate = DateTime.Now,
                        CreateUserId = OperatorProvider.Provider.Current().UserId,
                        OrganizeName = organ.FullName,
                        JobName = user.PostName,
                        WorkType = 1
                    };
                    res.Insert<WorkRecordEntity>(workEntity);
                }
                else
                {
                    entity.Create();
                    entity.OutDeptCode = dept.EnCode;
                    entity.CreateUserName = OperatorProvider.Provider.Current().UserName;
                    res.Insert<TransferEntity>(entity);
                    //如果需要确认则修改用户状态
                    user.IsTransfer = 1;

                }
                if (!string.IsNullOrEmpty(user.DepartmentId))
                {
                    DepartmentEntity depart = new BaseManage.DepartmentService().GetEntity(user.DepartmentId);
                    if (depart != null)
                    {
                        //重新处理承包商级用户关联的部门信息
                        if (depart.Nature == "承包商")
                        {
                            sql = string.Format("select d.departmentid,d.encode from BASE_DEPARTMENT d where d.parentid=(select t.departmentid from BASE_DEPARTMENT t where t.organizeid='{0}' and t.description='外包工程承包商') and instr('{1}',d.encode)>0", user.OrganizeId, depart.EnCode);
                            DataTable dtDept = BaseRepository().FindTable(sql);
                            if (dtDept.Rows.Count > 0)
                            {
                                user.NickName = depart.DepartmentId; //存储承包商或下属部门的Id
                            }
                        }
                        else
                        {
                            user.NickName = user.DepartmentId;
                        }
                    }
                }
                user.Modify(user.UserId);
                res.Update<UserEntity>(user);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }

        }



        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        public void AppSaveForm(string keyValue, TransferEntity entity, string Userid)
        {

            //开始事物
            var res = DbFactory.Base().BeginTrans();
            try
            {
                Repository<UserEntity> inlogdb = new Repository<UserEntity>(DbFactory.Base());
                string sql = string.Format("select * from base_user  where userid ='{0}' ", entity.UserId);
                UserEntity user = inlogdb.FindList(sql).FirstOrDefault();
                inlogdb = new Repository<UserEntity>(DbFactory.Base());
                string Currentsql = string.Format("select * from base_user  where userid ='{0}' ", Userid);
                UserEntity Currentuser = inlogdb.FindList(Currentsql).FirstOrDefault();

                Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", entity.OutDeptId);
                DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();

                if (keyValue == "0")
                {
                    //entity.TID = Guid.NewGuid().ToString();
                    entity.CreateDate = DateTime.Now;
                    entity.CreateUserId = Userid;
                    entity.CreateUserDeptCode = Currentuser.DepartmentCode;
                    entity.CreateUserOrgCode = Currentuser.OrganizeCode;
                    entity.OutDeptCode = dept.EnCode;
                    entity.CreateUserName = Currentuser.RealName;
                    res.Insert<TransferEntity>(entity);
                }
                else
                {
                    Repository<TransferEntity> trandb = new Repository<TransferEntity>(DbFactory.Base());
                    string transql = string.Format("select * from bis_Transfer where Tid='{0}' ", entity.TID);
                    TransferEntity tran = trandb.FindList(transql).FirstOrDefault();

                    tran.OutDeptCode = entity.OutDeptCode;
                    tran.OutDeptId = entity.OutDeptId;
                    tran.OutDeptName = entity.OutDeptName;
                    tran.OutJobId = entity.OutJobId;
                    tran.OutJobName = entity.OutJobName;
                    tran.OutPostId = entity.OutPostId;
                    tran.OutPostName = entity.OutPostName;
                    tran.TransferTime = entity.TransferTime;
                    tran.UserId = entity.UserId;
                    tran.UserName = entity.UserName;
                    //entity.TID = keyValue;
                    tran.ModifyDate = DateTime.Now;
                    tran.ModifyUserId = Userid;
                    res.Update<TransferEntity>(tran);

                }

                if (entity.IsConfirm == 0 || entity.IsConfirm == 2)
                {



                    //根据部门id获取所属机构Code
                    Repository<DepartmentEntity> organdb = new Repository<DepartmentEntity>(DbFactory.Base());
                    string organsql = string.Format("select * from base_department  where   departmentid='{0}' ",
                        dept.OrganizeId);
                    DepartmentEntity organ = organdb.FindList(organsql).FirstOrDefault();
                    user.OrganizeCode = organ.EnCode;

                    if (entity.OutJobId != null && entity.OutJobId != "")
                    {
                        string postcode = "";
                        Repository<RoleEntity> postdb = new Repository<RoleEntity>(DbFactory.Base());
                        string postsql = string.Format("select * from base_role where category=3 ");
                        IEnumerable<RoleEntity> rlist = postdb.FindList(postsql);
                        string[] Postids = entity.OutJobId.Split(',');
                        for (int i = 0; i < Postids.Length; i++)
                        {
                            RoleEntity ro = rlist.Where(it => it.RoleId == Postids[i]).FirstOrDefault();
                            if (ro != null)
                            {
                                if (postcode == "")
                                {
                                    postcode = ro.EnCode;
                                }
                                else
                                {
                                    postcode += "," + ro.EnCode;
                                }
                            }
                        }

                        user.PostCode = postcode;
                    }

                    //如果不需要确认 则直接修改用户的部门 岗位 职务信息
                    user.PostName = entity.OutJobName;
                    user.PostId = entity.OutJobId;
                    user.DutyId = entity.OutPostId;
                    user.DutyName = entity.OutPostName;
                    user.DepartmentId = entity.OutDeptId;
                    user.DepartmentCode = dept.EnCode;
                    user.OrganizeId = dept.OrganizeId;
                    user.IsTransfer = 0;

                    #region 给转岗用户赋默认角色
                    string roleName = "";
                    string roleId = "";
                    Repository<RoleEntity> roledb = new Repository<RoleEntity>(DbFactory.Base());
                    //如果选择的是厂级部门的话，角色会默认追加“厂级部门用户”
                    if (!(string.IsNullOrEmpty(user.DepartmentId) || user.DepartmentId == "undefined"))
                    {
                        Repository<DepartmentEntity> roledeptandb = new Repository<DepartmentEntity>(DbFactory.Base());
                        if (roledeptandb.FindEntity(user.DepartmentId).IsOrg == 1)
                        {
                            roleName += "厂级部门用户,";


                            var expression = LinqExtensions.True<RoleEntity>();
                            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                            RoleEntity cj = roledb.IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.FullName == "厂级部门用户").FirstOrDefault();
                            if (cj != null)
                                roleId += cj.RoleId + ",";
                        }
                    }


                    var expression1 = LinqExtensions.True<RoleEntity>();
                    expression1 = expression1.And(t => t.Category == 2).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                    IEnumerable<RoleEntity> role = roledb.IQueryable(expression1).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.RoleId == user.DutyId);
                    RoleEntity roleentity = role.FirstOrDefault();
                    if (roleentity != null)
                    {
                        roleName += roleentity.RoleNames;
                        roleId += roleentity.RoleIds;
                    }
                    user.RoleId = roleId;
                    user.RoleName = roleName;
                    #endregion

                    #region 默认添加 角色、岗位、职位
                    res.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == user.UserId);
                    List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                    var currUser = OperatorProvider.Provider.Current();
                    string uid = currUser == null ? "" : currUser.UserId;
                    string uname = currUser == null ? "" : currUser.UserName;
                    //用户
                    userRelationEntitys.Add(new UserRelationEntity
                    {
                        Category = 6,
                        UserRelationId = Guid.NewGuid().ToString(),
                        UserId = user.UserId,
                        ObjectId = user.UserId,
                        CreateDate = DateTime.Now,
                        CreateUserId = uid,
                        CreateUserName = uname,
                        IsDefault = 1,
                    });
                    //角色
                    string[] arr = user.RoleId.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in arr)
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 2,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = item,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    //岗位
                    if (!string.IsNullOrEmpty(user.DutyId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 3,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = user.DutyId,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    //职位
                    if (!string.IsNullOrEmpty(user.PostId))
                    {
                        userRelationEntitys.Add(new UserRelationEntity
                        {
                            Category = 4,
                            UserRelationId = Guid.NewGuid().ToString(),
                            UserId = user.UserId,
                            ObjectId = user.PostId,
                            CreateDate = DateTime.Now,
                            CreateUserId = uid,
                            CreateUserName = uname,
                            IsDefault = 1,
                        });
                    }
                    res.Insert<UserRelationEntity>(userRelationEntitys);
                    #endregion

                    //转岗完成将记录加入到记录表中
                    Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
                    //找到之前没有结尾的工作记录填写结束时间
                    string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, entity.InDeptId, entity.TransferTime);
                    res.ExecuteBySql(Worksql);
                    //新增一条新的岗位记录
                    WorkRecordEntity workEntity = new WorkRecordEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        DeptCode = user.DepartmentCode,
                        DeptId = user.DepartmentId,
                        EnterDate = Convert.ToDateTime(entity.TransferTime),
                        UserId = user.UserId,
                        UserName = user.RealName,
                        DeptName = entity.OutDeptName,
                        PostName = user.DutyName,
                        CreateDate = DateTime.Now,
                        CreateUserId = Userid,
                        OrganizeName = organ.FullName,
                        JobName = user.PostName,
                        WorkType = 1
                    };
                    res.Insert<WorkRecordEntity>(workEntity);
                }
                else
                {
                    //如果需要确认则修改用户状态
                    user.IsTransfer = 1;

                }
                user.Modify(user.UserId);
                res.Update<UserEntity>(user);
                res.Commit();
            }
            catch (Exception ex)
            {
                res.Rollback();
                throw ex;
            }

        }



        ///// <summary>
        ///// 转岗确认操作
        ///// </summary>
        ///// <param name="keyValue"></param>
        ///// <param name="entity"></param>
        //public void Update(string keyValue, TransferEntity entity)
        //{
        //    //开始事物
        //    var res = DbFactory.Base().BeginTrans();
        //    try
        //    {
        //        Repository<UserEntity> inlogdb = new Repository<UserEntity>(DbFactory.Base());
        //        string sql = string.Format("select * from base_user  where userid ='{0}' ", entity.UserId);
        //        UserEntity user = inlogdb.FindList(sql).FirstOrDefault();
        //        Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
        //        string deptsql = string.Format("select * from base_Department  where departmentid ='{0}' ", entity.OutDeptId);
        //        DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
        //        entity.Modify(keyValue);
        //        //confirm为2的时候表示已完成
        //        entity.IsConfirm = 2;
        //        res.Update<TransferEntity>(entity);


        //        //根据部门id获取所属机构Code
        //        Repository<OrganizeEntity> organdb = new Repository<OrganizeEntity>(DbFactory.Base());
        //        string organsql = string.Format("select * from base_organize where  organizeid='{0}' ",
        //            entity.OutDeptId);
        //        OrganizeEntity organ = organdb.FindList(organsql).FirstOrDefault();
        //        user.OrganizeCode = organ.EnCode;


        //        //如果不需要确认 则直接修改用户的部门 岗位 职务信息
        //        user.PostName = entity.OutJobName;
        //        user.PostId = entity.OutJobId;
        //        user.DutyId = entity.OutPostId;
        //        user.DutyName = entity.OutPostName;
        //        user.DepartmentId = entity.OutDeptId;
        //        user.DepartmentCode = dept.DeptCode;
        //        user.OrganizeId = dept.OrganizeId;
        //        user.IsTransfer = 0;

        //        //转岗完成将记录加入到记录表中
        //        Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
        //        //找到之前没有结尾的工作记录填写结束时间
        //        string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime is null  order by createdate desc) a where rownum=1) ", user.UserId, user.DepartmentId, entity.TransferTime);
        //        res.ExecuteBySql(Worksql);
        //        //新增一条新的岗位记录
        //        WorkRecordEntity workEntity = new WorkRecordEntity
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            DeptCode = user.DepartmentCode,
        //            DeptId = user.DepartmentId,
        //            EnterDate = Convert.ToDateTime(entity.TransferTime),
        //            UserId = user.UserId,
        //            UserName = user.RealName,
        //            DeptName = entity.OutDeptName,
        //            PostName = user.DutyName,
        //            CreateDate = DateTime.Now,
        //            CreateUserId = OperatorProvider.Provider.Current().UserId,
        //            OrganizeName = organ.FullName,
        //            JobName = user.PostName,
        //            WorkType = 0
        //        };
        //        res.Insert<WorkRecordEntity>(workEntity);

        //        user.Modify(user.UserId);
        //        res.Update<UserEntity>(user);
        //        res.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Rollback();
        //        throw ex;
        //    }
        //}

        #endregion
    }
}
