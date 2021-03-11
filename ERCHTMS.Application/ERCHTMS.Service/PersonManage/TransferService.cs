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
    /// �� ����ת����Ϣ��
    /// </summary>
    public class TransferService : RepositoryFactory<TransferEntity>, TransferIService
    {
        #region ��ȡ����
        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public IEnumerable<TransferEntity> GetList(string queryJson)
        {
            return this.BaseRepository().IQueryable().ToList();
        }
        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <returns></returns>
        public TransferEntity GetEntity(string keyValue)
        {
            return this.BaseRepository().FindEntity(keyValue);
        }


        /// <summary>
        /// ��ȡʵ��
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
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
                    if (dept.FullName != tr.OutDeptName)//����ͬ����������Ϣ�᱾��������� �����xx|xx�ظ�����
                    {
                        tr.OutDeptName = dept.FullName + "|" + tr.OutDeptName;
                    }
                }

                if (dept.Nature == "����" || dept.Nature == "רҵ")
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
        /// ���ݵ�ǰ����id��ȡ�㼶��ʾ����
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

                    if (dept.Nature == "����" || dept.Nature == "רҵ")
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
        /// ��ȡ��ǰ�û�����ת�ڴ�������
        /// </summary>
        /// <returns></returns>
        public int GetTransferNum()
        {
            try
            {
                //��ȡ����ǰ��¼�û�
                Operator user = OperatorProvider.Provider.Current();
                string sql = "select count(TID) from bis_transfer ";
                string where = "";

                string[] role = user.RoleName.Split(',');
                //�����ж��Ƿ��ǰ���\רҵ\���Ÿ�����
                if (role.Contains("������") || role.Contains("��˾�쵼") || role.Contains("���������û�"))
                {
                    //����Ǹ����˼����� ���ѯ�����ż������������ŵ�ת������
                    where = string.Format("where IsConfirm=1  and outdeptcode like '{0}%'", user.DeptCode);
                }
                else
                {
                    //�������ͨԱ��ֻ�ܲ鿴�¼����ŵ�ת������
                    where = string.Format("where IsConfirm=1  and outdeptcode like '{0}%' and outdeptcode != '{0}'", user.DeptCode);
                    //Repository<DepartmentEntity> deptdb = new Repository<DepartmentEntity>(DbFactory.Base());
                    //string deptsql = string.Format("select * from base_Department  where parentid ='{0}' ", user.DeptId);
                    //DepartmentEntity dept = deptdb.FindList(deptsql).FirstOrDefault();
                    //if (dept != null)
                    //{
                    //    //�������ͨԱ��ֻ�ܲ鿴�¼����ŵ�ת������
                    //    where = string.Format("where outdeptcode like '{0}%'", dept.DeptCode);
                    //}
                    //else
                    //{
                    //    //���û���¼������򷵻�0;
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
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="queryJson">��ѯ����</param>
        /// <returns>�����б�</returns>
        public DataTable GetTransferList(Pagination pagination, string queryJson)
        {
            try
            {
                DatabaseType dataType = DbHelper.DbType;
                var queryParam = queryJson.ToJObject();
                //��ѯ����
                if (!queryParam["Name"].IsEmpty())//Σ������
                {
                    string Name = queryParam["Name"].ToString();
                    pagination.conditionJson += string.Format(" and UserName  like '%{0}%'", Name.Trim());
                }

                //��ȡ����ǰ��¼�û�
                Operator user = OperatorProvider.Provider.Current();
                string[] role = user.RoleName.Split(',');
                //�����ж��Ƿ��ǰ���\רҵ\���Ÿ�����
                if (role.Contains("������") || role.Contains("��˾�쵼") || role.Contains("���������û�"))
                {
                    //����Ǹ����˼����� ���ѯ�����ż������������ŵ�ת������
                    pagination.conditionJson += string.Format(" and outdeptcode like '{0}%'", user.DeptCode);
                }
                else
                {
                    //�������ͨԱ��ֻ�ܲ鿴�¼����ŵ�ת������
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

        #region �ύ����
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="keyValue">����</param>
        public void RemoveForm(string keyValue)
        {
            this.BaseRepository().Delete(keyValue);
        }
        /// <summary>
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void SaveForm(string keyValue, TransferEntity entity)
        {

            //��ʼ����
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
                        //confirmΪ2��ʱ���ʾ�����
                        entity.IsConfirm = 2;
                        res.Update<TransferEntity>(entity);

                    }

                    //���ݲ���id��ȡ��������Code
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

                    //�������Ҫȷ�� ��ֱ���޸��û��Ĳ��� ��λ ְ����Ϣ
                    
                    user.DutyId = entity.OutPostId;
                    user.DutyName = entity.OutPostName;
                    user.DepartmentId = entity.OutDeptId;
                    user.DepartmentCode = dept.EnCode;

                    #region ��ת���û���Ĭ�Ͻ�ɫ
                    string roleName = "";
                    string roleId = "";
                    Repository<RoleEntity> roledb = new Repository<RoleEntity>(DbFactory.Base());
                    //���ѡ����ǳ������ŵĻ�����ɫ��Ĭ��׷�ӡ����������û���
                    if (!(string.IsNullOrEmpty(user.DepartmentId) || user.DepartmentId == "undefined"))
                    {
                        Repository<DepartmentEntity> roledeptandb = new Repository<DepartmentEntity>(DbFactory.Base());
                        if (roledeptandb.FindEntity(user.DepartmentId).IsOrg == 1)
                        {
                            roleName += "���������û�,";

                            
                            var expression = LinqExtensions.True<RoleEntity>();
                            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                            RoleEntity cj = roledb.IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.FullName == "���������û�").FirstOrDefault();
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

                    #region Ĭ����� ��ɫ����λ��ְλ
                    res.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == user.UserId);
                    List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                    var currUser = OperatorProvider.Provider.Current();
                    string uid = currUser == null ? "" : currUser.UserId;
                    string uname = currUser == null ? "" : currUser.UserName;
                    //�û�
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
                    //��ɫ
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
                    //��λ
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
                    //ְλ
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
                    //ת����ɽ���¼���뵽��¼����
                    Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
                    //�ҵ�֮ǰû�н�β�Ĺ�����¼��д����ʱ��
                    string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, entity.InDeptId, entity.TransferTime);
                    res.ExecuteBySql(Worksql);

                    #region ����ǰ��齫��������ת���� ����/����
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
                            if (dept1.FullName != DeptName)//����ͬ����������Ϣ�᱾��������� �����xx|xx�ظ�����
                            {
                                DeptName = dept1.FullName + "|" + DeptName;
                            }
                        }

                        if (dept1.Nature == "����" || dept1.Nature == "רҵ")
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
                    //����һ���µĸ�λ��¼
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
                    //�����Ҫȷ�����޸��û�״̬
                    user.IsTransfer = 1;

                }
                if (!string.IsNullOrEmpty(user.DepartmentId))
                {
                    DepartmentEntity depart = new BaseManage.DepartmentService().GetEntity(user.DepartmentId);
                    if (depart != null)
                    {
                        //���´���а��̼��û������Ĳ�����Ϣ
                        if (depart.Nature == "�а���")
                        {
                            sql = string.Format("select d.departmentid,d.encode from BASE_DEPARTMENT d where d.parentid=(select t.departmentid from BASE_DEPARTMENT t where t.organizeid='{0}' and t.description='������̳а���') and instr('{1}',d.encode)>0", user.OrganizeId, depart.EnCode);
                            DataTable dtDept = BaseRepository().FindTable(sql);
                            if (dtDept.Rows.Count > 0)
                            {
                                user.NickName = depart.DepartmentId; //�洢�а��̻��������ŵ�Id
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
        /// ��������������޸ģ�
        /// </summary>
        /// <param name="keyValue">����ֵ</param>
        /// <param name="entity">ʵ�����</param>
        /// <returns></returns>
        public void AppSaveForm(string keyValue, TransferEntity entity, string Userid)
        {

            //��ʼ����
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



                    //���ݲ���id��ȡ��������Code
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

                    //�������Ҫȷ�� ��ֱ���޸��û��Ĳ��� ��λ ְ����Ϣ
                    user.PostName = entity.OutJobName;
                    user.PostId = entity.OutJobId;
                    user.DutyId = entity.OutPostId;
                    user.DutyName = entity.OutPostName;
                    user.DepartmentId = entity.OutDeptId;
                    user.DepartmentCode = dept.EnCode;
                    user.OrganizeId = dept.OrganizeId;
                    user.IsTransfer = 0;

                    #region ��ת���û���Ĭ�Ͻ�ɫ
                    string roleName = "";
                    string roleId = "";
                    Repository<RoleEntity> roledb = new Repository<RoleEntity>(DbFactory.Base());
                    //���ѡ����ǳ������ŵĻ�����ɫ��Ĭ��׷�ӡ����������û���
                    if (!(string.IsNullOrEmpty(user.DepartmentId) || user.DepartmentId == "undefined"))
                    {
                        Repository<DepartmentEntity> roledeptandb = new Repository<DepartmentEntity>(DbFactory.Base());
                        if (roledeptandb.FindEntity(user.DepartmentId).IsOrg == 1)
                        {
                            roleName += "���������û�,";


                            var expression = LinqExtensions.True<RoleEntity>();
                            expression = expression.And(t => t.Category == 1).And(t => t.EnabledMark == 1).And(t => t.DeleteMark == 0);
                            RoleEntity cj = roledb.IQueryable(expression).OrderByDescending(t => t.CreateDate).ToList().Where(a => a.FullName == "���������û�").FirstOrDefault();
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

                    #region Ĭ����� ��ɫ����λ��ְλ
                    res.Delete<UserRelationEntity>(t => t.IsDefault == 1 && t.UserId == user.UserId);
                    List<UserRelationEntity> userRelationEntitys = new List<UserRelationEntity>();
                    var currUser = OperatorProvider.Provider.Current();
                    string uid = currUser == null ? "" : currUser.UserId;
                    string uname = currUser == null ? "" : currUser.UserName;
                    //�û�
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
                    //��ɫ
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
                    //��λ
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
                    //ְλ
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

                    //ת����ɽ���¼���뵽��¼����
                    Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
                    //�ҵ�֮ǰû�н�β�Ĺ�����¼��д����ʱ��
                    string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime =to_date('0001-01-01 00:00:00','yyyy-mm-dd hh24:mi:ss')  order by createdate desc) a where rownum=1) ", user.UserId, entity.InDeptId, entity.TransferTime);
                    res.ExecuteBySql(Worksql);
                    //����һ���µĸ�λ��¼
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
                    //�����Ҫȷ�����޸��û�״̬
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
        ///// ת��ȷ�ϲ���
        ///// </summary>
        ///// <param name="keyValue"></param>
        ///// <param name="entity"></param>
        //public void Update(string keyValue, TransferEntity entity)
        //{
        //    //��ʼ����
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
        //        //confirmΪ2��ʱ���ʾ�����
        //        entity.IsConfirm = 2;
        //        res.Update<TransferEntity>(entity);


        //        //���ݲ���id��ȡ��������Code
        //        Repository<OrganizeEntity> organdb = new Repository<OrganizeEntity>(DbFactory.Base());
        //        string organsql = string.Format("select * from base_organize where  organizeid='{0}' ",
        //            entity.OutDeptId);
        //        OrganizeEntity organ = organdb.FindList(organsql).FirstOrDefault();
        //        user.OrganizeCode = organ.EnCode;


        //        //�������Ҫȷ�� ��ֱ���޸��û��Ĳ��� ��λ ְ����Ϣ
        //        user.PostName = entity.OutJobName;
        //        user.PostId = entity.OutJobId;
        //        user.DutyId = entity.OutPostId;
        //        user.DutyName = entity.OutPostName;
        //        user.DepartmentId = entity.OutDeptId;
        //        user.DepartmentCode = dept.DeptCode;
        //        user.OrganizeId = dept.OrganizeId;
        //        user.IsTransfer = 0;

        //        //ת����ɽ���¼���뵽��¼����
        //        Repository<WorkRecordEntity> workdb = new Repository<WorkRecordEntity>(DbFactory.Base());
        //        //�ҵ�֮ǰû�н�β�Ĺ�����¼��д����ʱ��
        //        string Worksql = string.Format("update BIS_WORKRECORD set leavetime=to_date('{2}','yyyy-mm-dd hh24:mi:ss') where id=(select id  from (select id from BIS_WORKRECORD t where userid='{0}' and deptid='{1}'  and WorkType=1 and LeaveTime is null  order by createdate desc) a where rownum=1) ", user.UserId, user.DepartmentId, entity.TransferTime);
        //        res.ExecuteBySql(Worksql);
        //        //����һ���µĸ�λ��¼
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
