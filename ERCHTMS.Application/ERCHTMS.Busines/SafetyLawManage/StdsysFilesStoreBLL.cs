using ERCHTMS.Entity.NosaManage;
using ERCHTMS.IService.NosaManage;
using ERCHTMS.Service.NosaManage;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using System.Data;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Business;
using ERCHTMS.Entity.SafetyLawManage;

namespace ERCHTMS.Busines.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度收藏文件
    /// </summary>
    public class StdsysFilesStoreBLL:BaseBLL<StdsysFilesStoreEntity>
    {
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            var queryParam = queryJson.ToJObject();
            if (pagination.p_fields.IsEmpty())
            {
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,userid,stdsysid";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_stdsysfilesstore";
            pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
                        
            return base.GetList(pagination, queryJson);
        }
    }
}
