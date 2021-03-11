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

namespace ERCHTMS.Busines.SafetyLawManage
{
    /// <summary>
    /// 描 述：标准制度文件
    /// </summary>
    public class StdsysFilesBLL:BaseBLL<StdsysFilesEntity>
    {
        /// <summary>
        /// 获取分页数据
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
                pagination.p_fields = @"createdate,createuserid,createuserdeptcode,createuserorgcode,modifydate,modifyuserid,filename,fileno,refid,refname,pubdate,revisedate,usedate,pubdepartid,pubdepartname,pubuserid,pubusername,remark";
            }
            pagination.p_kid = "id";
            pagination.p_tablename = @"hrs_stdsysfiles";
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
                pagination.conditionJson += string.Format(@" and refid = '{0}'", queryParam["refid"].ToString());
            }
            //引用id(递归查询)
            if (!queryParam["type"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and refid in(select id from hrs_stdsystype start with id='{0}' connect by  prior id = parentid)", queryParam["type"].ToString());
            }
            //本单位
            if (!queryParam["deptcode"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and refid in(select id from hrs_stdsystype where scope like '{0}%')", queryParam["deptcode"].ToString());
            }
            //我的收藏
            if (!queryParam["mystore"].IsEmpty())
            {
                pagination.conditionJson += string.Format(@" and exists(select 1 from hrs_stdsysstorefiles s where s.stdsysid=hrs_stdsysfiles.id and s.userid='{0}')", user.UserId);
            }

            return base.GetList(pagination, queryJson);
        }
        /// <summary>
        /// 判断是否存在同名文件
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="fileName"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public bool ExistSame(string orgCode,string fileName,string keyValue)
        {
            var oldList = this.GetList(String.Format(" and createuserorgcode='{0}' and filename='{1}' and id<>'{2}'", orgCode, fileName, keyValue)).ToList();
            var r = oldList.Count > 0;

            return r;
        }
    }
}
