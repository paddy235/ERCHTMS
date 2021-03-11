using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RoutineSafetyWork;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class FileManageController : BaseApiController
    {
        public HttpContext ctx { get { return HttpContext.Current; } }
        private FileManageBLL FileManage = new FileManageBLL();
        private FileTreeManageBLL FileTreeManage = new FileTreeManageBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        /// <summary>
        /// 获取文件管理列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFileList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                //获取用户Id
                string userId = dy.userid;
                //获取文件类型ID
                string filetypeid = dy.data.filetypeid;
                //获取文件名称
                string filename = dy.data.filename;
                //是否包含子节点数据,0否,1是
                string type = dy.data.type;
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                //获取页数和条数
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                Pagination pagination = new Pagination();
                pagination.p_kid = "ID";
                pagination.p_fields = "FileName,FileNo,FileTypeName,ReleaseDeptName,to_char(ReleaseTime,'yyyy-Mm-dd') as ReleaseTime,Remark";
                pagination.p_tablename = "bis_filemanage t";
                pagination.page = page;//页数
                pagination.rows = rows;//行数
                pagination.sidx = "ReleaseTime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson = string.Format(" CreateUserOrgCode='{0}'", curUser.OrganizeCode);
                if (!string.IsNullOrEmpty(filetypeid)) {
                    if (type == "1")
                    {
                        pagination.conditionJson = string.Format("FileTypeCode like '{0}%'", filetypeid);
                    }
                    else {
                        pagination.conditionJson = string.Format("FileTypeCode='{0}'", filetypeid);
                    }

                }
                if (!string.IsNullOrEmpty(filename))
                {
                    pagination.conditionJson += string.Format(" and filename like '%{0}%' ", filename);
                }
                DataTable dt = FileManage.GetList(pagination, "app");
                List<object> data = new List<object>();
                foreach (DataRow dr in dt.Rows)
                {
                    dynamic entity = new ExpandoObject();
                    entity.filename = dr["FileName"].ToString();//文件名称
                    entity.fileno = dr["FileNo"].ToString();//文件编号
                    entity.filetypename = dr["FileTypeName"].ToString();//文件类型
                    entity.releasedeptname = dr["ReleaseDeptName"].ToString();//发布单位
                    entity.releasetime = dr["ReleaseTime"].ToString();//发布时间
                    entity.remark = dr["remark"].ToString();//备注
                    IList<Photo> rList = new List<Photo>();
                    //附件
                    DataTable file = fileInfoBLL.GetFiles(dr["ID"].ToString());
                    foreach (DataRow drs in file.Rows)
                    {
                        Photo p = new Photo();
                        p.id = drs["fileid"].ToString();
                        p.filename = drs["filename"].ToString();
                        p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + drs["filepath"].ToString().Substring(1);
                        rList.Add(p);
                    }
                    entity.file = rList;
                    data.Add(entity);
                }
                return new { code = 0, info = "获取数据成功", count = pagination.records, data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }


        }

        /// <summary>
        /// 获取标准制度所有类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetFileType([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dyy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dyy.userid;
            //获取用户基本信息
           
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
            }
            var where = string.Format(" and CreateUserOrgCode='{0}'", curUser.OrganizeCode);
            var data = FileTreeManage.GetList(where).OrderBy(t => t.CREATEDATE).ToList();
            List<object> dataList = new List<object>();
            foreach (var item in data)
            {
                bool hasChild = data.Where(x => x.ParentId == item.ID).Count() > 0 ? true : false;
                dynamic tree = new ExpandoObject();
                tree.id = item.ID;
                tree.name = item.TreeName;
                tree.code = item.TreeCode;
                tree.parentid = item.ParentId;
                tree.haschildren = hasChild;
                dataList.Add(tree);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss", //格式化日期
            };
            var result = new { code = 0, info = "获取数据成功", count = data.Count, data = dataList };

            return JObject.Parse(JsonConvert.SerializeObject(result, Formatting.None, settings));
        }
    }
}