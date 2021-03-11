using ERCHTMS.Entity.RoutineSafetyWork;
using ERCHTMS.IService.RoutineSafetyWork;
using ERCHTMS.Service.RoutineSafetyWork;
using BSFramework.Util.WebControl;
using System.Collections.Generic;
using System;
using BSFramework.Util;
using BSFramework.Util.Extension;
using System.Data;
using ERCHTMS.Code;
using ERCHTMS.Business;

namespace ERCHTMS.Busines.RoutineSafetyWork
{
    /// <summary>
    /// 描 述：文件管理
    /// </summary>
    public class FileManageBLL : BaseBLL<FileManageEntity>
    {
        //private FileManageIService service = new FileManageService();

        #region 获取数据
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public override DataTable GetList(Pagination pagination, string queryJson)
        {
            if (queryJson == "app")
            {
                return base.GetList(pagination, null);
            }
            else {
                Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                var queryParam = queryJson.ToJObject();
                if (pagination.p_fields.IsEmpty())
                {
                    pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,fileno,filetypeid,filetypename,to_char(releasetime,'yyyy-MM-dd') as releasetime,releasedeptname,remark";
                }
                pagination.p_kid = "id";
                pagination.p_tablename = @"bis_filemanage";
                pagination.conditionJson = string.Format(" createuserorgcode='{0}'", user.OrganizeCode);
                //编号
                if (!queryParam["fileno"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and fileno like '%{0}%'", queryParam["fileno"].ToString());
                }
                //名称
                if (!queryParam["filename"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and filename like '%{0}%'", queryParam["filename"].ToString());
                }
                //引用id(直接查询)
                if (!queryParam["refid"].IsEmpty())
                {
                    if (!queryParam["flag"].IsEmpty())
                    {
                        if (queryParam["flag"].ToString() == "0")
                        {
                            pagination.conditionJson += string.Format(@" and filetypecode = '{0}'", queryParam["refid"].ToString());
                        }
                        else
                        {
                            pagination.conditionJson += string.Format(@" and filetypecode like '{0}%'", queryParam["refid"].ToString());
                        }
                    }
                    else {
                        pagination.conditionJson += string.Format(@" and filetypecode = '{0}'", queryParam["refid"].ToString());
                    }
                    
                }
                //我的收藏
                if (!queryParam["mystore"].IsEmpty())
                {
                    pagination.conditionJson += string.Format(@" and exists(select 1 from BIS_FileManageStore s where s.FileManageId=bis_filemanage.id and s.userid='{0}')", user.UserId);
                }

                return base.GetList(pagination, queryJson);
            }
            
        }
        
        #endregion
    }
}
