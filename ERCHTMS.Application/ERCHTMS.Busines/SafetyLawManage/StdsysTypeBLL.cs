using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Business;
using ERCHTMS.Entity.SafetyLawManage;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.Busines.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度类型
    /// </summary>
    public class StdsysTypeBLL:BaseBLL<StdsysTypeEntity>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,name,code,parentid,scope,remark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_stdsystype";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
            //编号
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and code like '%{0}%'", queryParam["code"].ToString());
            }
            //名称
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and name like '%{0}%'", queryParam["name"].ToString());
            }

            return base.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public override void RemoveForm(string keyValue)
        {
            BaseBLL<StdsysFilesEntity> sfBll = new BaseBLL<StdsysFilesEntity>();
            var list = sfBll.GetList(string.Format(" and refid in(select id from hrs_stdsystype start with id='{0}' connect by  prior id = parentid)", keyValue));
            foreach (var e in list)
            {
                sfBll.RemoveForm(e);
            }
            var l = this.GetList(string.Format(" and id in(select id from hrs_stdsystype start with id='{0}' connect by  prior id = parentid)", keyValue)).ToList();
            base.RemoveForm(l);
        }
        /// <summary>
        /// 同步电厂的班组
        /// </summary>
        public void SynchroOrg()
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();            
            var list = this.GetList(string.Format(" and createuserorgcode='{0}'", user.OrganizeCode));
            var stdEntity = list.Where(x => x.Scope == "05").FirstOrDefault();
            if (stdEntity != null)
            {
                var depts = new DepartmentBLL().GetDepts(user.OrganizeId).Where(x => x.Nature == "班组");
                foreach(var dept in depts)
                {
                    if (list.Count(x => x.ID == dept.DepartmentId)==0)
                    {
                        var entity = new StdsysTypeEntity()
                        {
                            ID = dept.DepartmentId,
                            Name = dept.FullName,
                            Scope = dept.EnCode,
                            Code = dept.EnCode,
                            ParentId = stdEntity.ID
                        };
                        this.SaveForm("", entity);
                        var parentId = entity.ID;

                        entity.ID = Guid.NewGuid().ToString();
                        entity.Name = "技术标准";
                        entity.ParentId = parentId;
                        this.SaveForm("", entity);

                        entity.ID = Guid.NewGuid().ToString();
                        entity.Name = "管理标准";
                        entity.ParentId = parentId;
                        this.SaveForm("", entity);

                        entity.ID = Guid.NewGuid().ToString();
                        entity.Name = "工作标准";
                        entity.ParentId = parentId;
                        this.SaveForm("", entity);
                    }
                }
            }
        }
    }
}
