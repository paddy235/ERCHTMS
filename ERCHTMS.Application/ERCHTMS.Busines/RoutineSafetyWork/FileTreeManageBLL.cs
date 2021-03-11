using BSFramework.Util.WebControl;
using ERCHTMS.Business;
using ERCHTMS.Code;
using ERCHTMS.Entity.RoutineSafetyWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSFramework.Util;
using BSFramework.Util.Extension;

namespace ERCHTMS.Busines.RoutineSafetyWork
{
    public class FileTreeManageBLL : BaseBLL<FileManageTreeEntity>
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,TreeName,TreeCode,parentid,remark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"BIS_FileManageTree";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
            //编号
            if (!queryParam["code"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and TreeCode like '%{0}%'", queryParam["code"].ToString());
            }
            //名称
            if (!queryParam["name"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and TreeName like '%{0}%'", queryParam["name"].ToString());
            }

            return base.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        public override void RemoveForm(string keyValue)
        {
            BaseBLL<FileManageEntity> fmBll = new BaseBLL<FileManageEntity>();
            var list = fmBll.GetList(string.Format(" and FileTypeId in(select id from BIS_FileManageTree start with id='{0}' connect by  prior id = parentid)", keyValue));
            foreach (var e in list)
            {
                fmBll.RemoveForm(e);
            }
            var l = this.GetList(string.Format(" and id in(select id from BIS_FileManageTree start with id='{0}' connect by  prior id = parentid)", keyValue)).ToList();
            base.RemoveForm(l);
        }
        
    }
}
