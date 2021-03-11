using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AuthorizeManage;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.Desktop;
using ERCHTMS.Busines.OutsourcingProject;
using ERCHTMS.Busines.PersonManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.RiskDatabase;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OutsourcingProject;
using ERCHTMS.Entity.PersonManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using ERCHTMS.Busines.JPush;
using ERCHTMS.Busines.Observerecord;
using ERCHTMS.Entity.Observerecord;
using ERCHTMS.Entity.SystemManage;

namespace ERCHTMS.AppSerivce.Controllers
{

    public class MessageController : BaseApiController
    {
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private MessageBLL messagebll = new MessageBLL();
        private MessageDetailBLL messagedetailbll = new MessageDetailBLL();
        private MessageSetBLL messagesetbll = new MessageSetBLL();
        private MessageUserSetBLL messageusersetbll = new MessageUserSetBLL();
        private FileInfoBLL fileinfobll = new FileInfoBLL();
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMessageList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                long pageIndex = dy.pageindex;
                long pageSize = dy.pagesize;
                string action = dy.data.action;//0待提交 1全部

                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "t.id";
                pagination.p_fields = @"t.title,d.status,t.senduser";
                pagination.sidx = "t.sendtime";//排序字段
                pagination.sord = "desc";//排序方式
                pagination.conditionJson += " 1=1 ";
                //未查看的数据
                if (action == "0")
                {
                    pagination.p_tablename = string.Format(@"base_message t left join (select t.messageid,t.status,t.useraccount from base_messagedetail t where t.useraccount='{0}' and t.status=0) d on d.messageid=t.id", curUser.Account);
                    pagination.conditionJson += string.Format(" and d.useraccount='{0}' and d.status=0 ", curUser.Account);
                }
                else
                {
                    pagination.p_tablename = string.Format(@"base_message t left join (select t.messageid,t.status,t.useraccount from base_messagedetail t where t.useraccount='{0}' and t.status=1) d on d.messageid=t.id", curUser.Account);
                    pagination.conditionJson += string.Format(" and ((d.useraccount='{0}' and d.status=1) or t.senduser='{0}') ", curUser.Account);
                }
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                });
                var data = messagebll.GetPageList(pagination, queryJson);
                return new { code = 0, count = pagination.records, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }

        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMessageInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string messageId = dy.data.messageId;
                MessageEntity entity = messagebll.GetEntity(messageId);

                //获取相关附件
                var files = new FileInfoBLL().GetFiles(messageId);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                var result = new
                {
                    message = entity,
                    files = files
                };
                return new
                {
                    code = 0,
                    count = 1,
                    info = "获取数据成功",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 获取未读短消息数量
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetMessCountByUserId([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var num = messagebll.GetMessCountByUserId(user.Account);
                return new
                {
                    code = 0,
                    count = 1,
                    info = "获取数据成功",
                    data = num
                };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 保存或者提交数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveOrCommitData()
        {
            try
            {
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deleteids = dy.data.deleteids;//删除附件id集合
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                //消息实体
                string messageentity = JsonConvert.SerializeObject(dy.data.messageentity);
                var entity = JsonConvert.DeserializeObject<MessageEntity>(messageentity);

                //保存成功之后推送消息
                if (messagebll.SaveForm(entity.Id, entity))
                {
                    JPushApi.PublicMessage(entity);
                }
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                //处理附件
                if (!string.IsNullOrEmpty(deleteids))
                {
                    DeleteFile(deleteids);
                }
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    for (int i = 0; i < files.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = files[i];
                        //原始文件名
                        string fileName = System.IO.Path.GetFileName(file.FileName);
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(fileName);
                        string fileGuid = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString();
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\Upfile";
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(newFilePath))
                        {
                            //保存文件
                            file.SaveAs(newFilePath);
                            //文件信息写入数据库
                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            fileInfoEntity.RecId = entity.Id;
                            fileInfoEntity.FileName = fileName;
                            fileInfoEntity.FilePath = "~/Resource/Upfile/" + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.TrimStart('.');
                            FileInfoBLL fileInfoBLL = new FileInfoBLL();
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
                return new { code = 0, count = 0, info = "操作成功" };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
            FileInfoBLL fileInfoBLL = new FileInfoBLL();
            bool result = false;

            if (!string.IsNullOrEmpty(fileInfoIds))
            {
                string ids = "";

                string[] strArray = fileInfoIds.Split(',');

                foreach (string s in strArray)
                {
                    ids += "'" + s + "',";
                    var entity = fileInfoBLL.GetEntity(s);
                    if (entity != null)
                    {
                        var filePath = HttpContext.Current.Server.MapPath(entity.FilePath);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                if (!string.IsNullOrEmpty(ids))
                {
                    ids = ids.Substring(0, ids.Length - 1);
                }
                int count = fileInfoBLL.DeleteFileForm(ids);

                result = count > 0 ? true : false;
            }

            return result;
        }


        [HttpPost]
        public object FlagRead([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                messagebll.FlagReadMessage(user.Account);
                return new
                {
                    code = 0,
                    count = 1,
                    info = "成功",
                };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }


        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object UpdateMessStatus([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string messageId = dy.data.messageId;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                var detail = messagedetailbll.GetEntity(user.Account, messageId);
                if (detail != null)
                {
                    detail.Status = 1;
                    detail.LookTime = DateTime.Now;
                    messagedetailbll.SaveForm(detail.Id, detail);
                }
                return new
                {
                    code = 0,
                    count = 1,
                    info = "成功",
                };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 获取消息类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetCategory([FromBody]JObject json)
        {
            List<object> data = new List<object>() { new { itemname = "高风险作业", itenvaule = "高风险作业" },
            new  {  itemname = "外包工程", itenvaule = "外包工程"  },
            new  {  itemname = "隐患排查", itenvaule = "隐患排查"  },
            new  {  itemname = "反违章", itenvaule = "反违章"  },
            new  {  itemname = "人员管理", itenvaule = "人员管理"  },
            new  {  itemname = "设备设施", itenvaule = "设备设施"  },
            new  {  itemname = "风险管控", itenvaule = "风险管控"  },
            new  {  itemname = "事故事件", itenvaule = "事故事件"  },
            new  {  itemname = "重大危险源", itenvaule = "重大危险源"  },
            new  {  itemname = "例行安全工作", itenvaule = "例行安全工作"  },
            new  {  itemname = "应急演练", itenvaule = "应急演练"  },
            new  {  itemname = "其它", itenvaule = "其它"  }};
            return new
                   {
                       code = 0,
                       count = 1,
                       info = "成功",
                       data = data
                   };

        }
    }
}