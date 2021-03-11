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
    public class FileManageStoreBLL : BaseBLL<FileManageStoreEntity>
    {
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,userid,FileManageId";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"BIS_FileManageStore";
            //pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);

            return base.GetList(pagination, queryJson);
        }
    }
}
