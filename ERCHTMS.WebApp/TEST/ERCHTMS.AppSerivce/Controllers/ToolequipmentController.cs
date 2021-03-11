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
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Busines.ToolEquipmentManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.PublicInfoManage;
using ERCHTMS.Entity.ToolEquipmentManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class ToolequipmentController : BaseApiController
    {
        private ToolequipmentBLL toolequipmentBll = new ToolequipmentBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        private ToolrecordBLL toolrecordbll = new ToolrecordBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        // GET api/toolequipment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/toolequipment/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/toolequipment
        public void Post([FromBody]string value)
        {
        }

        // PUT api/toolequipment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/toolequipment/5
        public void Delete(int id)
        {
        }
        /// <summary>
        /// 保存工器具
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object ToolSave()
        {
            try
            {
                string res = ctx.Request["json"];
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new ToolModel()
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                string keyValue = !string.IsNullOrEmpty(dyObj.data.keyvalue) ? dyObj.data.keyvalue : Guid.NewGuid().ToString();
                //var year = DateTime.Now.ToString("yyyy");
                //var month = DateTime.Now.ToString("MM");
                //var rewardCode = "Q/CRPHZHB 2208.06.01-JL01-" + year + month + safereward.GetRewardCode();
                //dyObj.data.entity.编号 = rewardCode;
                toolequipmentBll.SaveForm(keyValue, dyObj.data.entity);


                //删除图片
                string delFileIds = !string.IsNullOrEmpty(dyObj.data.delfileids) ? dyObj.data.delfileids : "";
                if (!string.IsNullOrEmpty(delFileIds))
                {
                    DeleteFile(delFileIds);
                }


                HttpFileCollection files = ctx.Request.Files;//上传的文件 
                //上传设备图片
                UploadifyFile(dyObj.data.entity.DescriptionFileId, dyObj.data.entity.ContractFileId,"", files);


            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }

        /// <summary>
        /// 保存工器具检查记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveToolRecord()
        {
            try
            {
                string res = ctx.Request["json"];
                //dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                var dyObj = JsonConvert.DeserializeAnonymousType(res, new
                {
                    userid = string.Empty,
                    data = new ToolrecordEntity(),
                    keyvalue = string.Empty
                });
                string userId = dyObj.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId; //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new {code = -1, count = 0, info = "请求失败,请登录!"};
                }

                string keyValue = !string.IsNullOrEmpty(dyObj.keyvalue) ? dyObj.keyvalue : Guid.NewGuid().ToString();
                toolequipmentBll.SaveToolrecord(keyValue, dyObj.data);


                HttpFileCollection files = ctx.Request.Files; //上传的文件 
                //上传设备图片
                UploadifyFile("", "", dyObj.data.Id, files);

            }
            catch (Exception ex)
            {
                return new {code = -1, count = 0, info = "保存失败"};
            }

            return new {code = 0, count = 0, info = "保存成功"};
        }


        /// <summary>
        /// 获取工器具列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public Object GetToolList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;


                //工器具大类
                string tooltype = res.Contains("tooltype") ? dy.data.tooltype : "";

                //开始日期
                string sTime = res.Contains("stime") ? dy.data.stime : "";
                //结束日期
                string eTime = res.Contains("etime") ? dy.data.etime : "";

                //模糊查询条件
                string keyword = res.Contains("keyword") ? dy.data.keyword : "";

                int pageSize = res.Contains("pagesize") ? int.Parse(dy.data.pagesize.ToString()) : 10; //每页条数

                int pageIndex = res.Contains("pagenum") ? int.Parse(dy.data.pagenum.ToString()) : 1; //请求页码

                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                Pagination pagination = new Pagination();
                pagination.page = int.Parse(pageIndex.ToString());
                pagination.rows = int.Parse(pageSize.ToString());
                pagination.p_kid = "Id";
                pagination.p_fields = @"b.Appraise,b.operuser,Equipmenttype,EquipmentName,EquipmentValue,EquipmentNo,Specifications,outputdeptname,checkdate,NextCheckDate,factorydate,district,districtid,districtcode,depositary,controluserid,controlusername,acceptance,ControlDept,ControlDeptId,ControlDeptCode,belongdept,belongdeptid,belongdeptCode,CreateUserId";
                pagination.p_tablename = @"BIS_TOOLEQUIPMENT a left join (select appraise,tb1.toolequipmentid,operuser from BIS_TOOLRECORD tb1 ,(select  max(createdate) createdate,toolequipmentid from BIS_TOOLRECORD group by toolequipmentid) tb2 
                where tb1.createdate= tb2.createdate  and tb1.toolequipmentid= tb2.toolequipmentid) b on a.id = b.toolequipmentid";
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.conditionJson = "1=1";
                string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    ToolType = tooltype,
                    startTime = sTime,
                    endTime = eTime,
                    condition = "Equipmentvalue",
                    txtSearch = keyword
                });
                var rewarddata = toolequipmentBll.GetPageList(pagination, queryJson);
                var data = new
                {
                    rows = rewarddata,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
                return new { Code = 0, Count = pagination.records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取工器具检查记录
        /// </summary>
        /// <returns></returns>
        public DataTable GetToolRecordList(string toolid, int pageIndex, int pageSize,out int records)
        {
            var watch = CommonHelper.TimerStart();
            Pagination pagination = new Pagination();
            pagination.page = pageIndex;
            pagination.rows = pageSize;
            pagination.p_kid = "id";
            pagination.p_fields = @"toolequipmentid,equipmentname,equipmentno,voltagelevel,trialvoltage,checkdate,nextcheckdate,appraise,operuser,specification,checkproject";
            pagination.p_tablename = "BIS_TOOLRECORD";
            pagination.sidx = "createdate";
            pagination.sord = "desc";
            pagination.conditionJson = "1=1";
            string queryJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                keyValue = toolid
            });
            var rewarddata = toolrecordbll.GetPageList(pagination, queryJson);
            records = pagination.records;
            return rewarddata;
        }



        /// <summary>
        /// 获取工器具详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetToolDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);

                int pageSize = res.Contains("pagesize") ? int.Parse(dy.data.pagesize.ToString()) : 10; //每页条数

                int pageIndex = res.Contains("pageindex") ? int.Parse(dy.data.pageindex.ToString()) : 1; //请求页码

                string userId = dy.userid;
                string keyValue = res.Contains("id") ? dy.data.id : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                ToolequipmentEntity toolequipment = toolequipmentBll.GetEntity(keyValue);
                object data = new object();
                int records = -1;
                if (toolequipment != null)
                {
                    object obj = new
                    {
                        id = toolequipment.Id,
                        createuserid = toolequipment.CreateUserId,
                        createuserdeptcode = toolequipment.CreateUserDeptCode,
                        createuserorgcode = toolequipment.CreateUserOrgCode,
                        createdate = toolequipment.CreateDate,
                        createusername = toolequipment.CreateUserName,
                        modifydate = toolequipment.ModifyDate,
                        modifyuserid = toolequipment.ModifyUserId,
                        modifyusername = toolequipment.ModifyUserName,
                        equipmentvalue = toolequipment.EquipmentValue,
                        equipmentname = toolequipment.EquipmentName,
                        equipmenttype = toolequipment.EquipmentType,
                        equipmentno = toolequipment.EquipmentNo,
                        securitymanageruser = toolequipment.SecurityManagerUser,
                        securitymanageruserid = toolequipment.SecurityManagerUserId,
                        telephone = toolequipment.Telephone,
                        specifications = toolequipment.Specifications,
                        district = toolequipment.District,
                        districtid = toolequipment.DistrictId,
                        districtcode = toolequipment.DistrictCode,
                        depositary = toolequipment.Depositary,
                        checkdate = toolequipment.CheckDate,
                        nextcheckdate = toolequipment.NextCheckDate,
                        validitydate = toolequipment.ValidityDate,
                        operuser = toolequipment.OperUser,
                        operuserid = toolequipment.OperUserId,
                        ischeck = toolequipment.IsCheck,
                        outputdeptname = toolequipment.OutputDeptName,
                        factoryno = toolequipment.FactoryNo,
                        factorydate = toolequipment.FactoryDate,
                        state = toolequipment.State,
                        controluserid = toolequipment.ControlUserId,
                        controlusername = toolequipment.ControlUserName,
                        controldept = toolequipment.ControlDept,
                        controldeptid = toolequipment.ControlDeptId,
                        controldeptcode = toolequipment.ControlDeptCode,
                        checkdatecycle = toolequipment.CheckDateCycle,
                        acceptance = toolequipment.Acceptance,
                        tooltype = toolequipment.ToolType,
                        appraise = toolequipment.Appraise,
                        descriptionfileid = toolequipment.DescriptionFileId,
                        contractfileid = toolequipment.ContractFileId,
                        belongdept = toolequipment.BelongDept,
                        belongdeptid = toolequipment.BelongDeptId,
                        belongdeptcode = toolequipment.BelongDeptCode
                    };
                    List<FileInfoEntity> descriptionfile = fileInfoBLL.GetFileList(toolequipment.DescriptionFileId);
                    List<FileInfoEntity> contractfile = fileInfoBLL.GetFileList(toolequipment.ContractFileId);
                    string strurl = new DataItemDetailBLL().GetItemValue("imgUrl");
                    List<object> objects = new List<object>();

                    foreach (FileInfoEntity itemEntity in descriptionfile)
                    {
                        objects.Add(new
                        {
                            fileid = itemEntity.FileId,
                            filepath = strurl + itemEntity.FilePath.Replace("~", ""),
                            filename = itemEntity.FileName,
                            recid = itemEntity.RecId
                        });

                    }

                    foreach (FileInfoEntity itemEntity in contractfile)
                    {
                        objects.Add(new
                        {
                            fileid = itemEntity.FileId,
                            filepath = strurl + itemEntity.FilePath.Replace("~", ""),
                            filename = itemEntity.FileName,
                            recid = itemEntity.RecId
                        });

                    }

 
                    DataTable recordlist = GetToolRecordList(toolequipment.Id, pageIndex, pageSize,out records);
                    List<object> recordfiles = new List<object>();
                    if (recordlist.Rows.Count > 0)
                    {
                        foreach (DataRow recordEntity in recordlist.Rows)
                        {
                            List<FileInfoEntity> fileinfos = fileInfoBLL.GetFileList(recordEntity["Id"].ToString());
                            if (fileinfos.Count > 0)
                            {
                                foreach (var fileInfoEntity in fileinfos)
                                {
                                    recordfiles.Add(new
                                    {
                                        fileid = fileInfoEntity.FileId,
                                        filepath = strurl + fileInfoEntity.FilePath.Replace("~", ""),
                                        filename = fileInfoEntity.FileName,
                                        recid = fileInfoEntity.RecId
                                    });
                                }
                            }
                        }
                    }

                    //检查记录信息与附件信息
                   object toolrecord = new
                    {
                        recordlist = recordlist,
                        recordfile = recordfiles
                    };
                    data = new
                    {
                        tooldetail = obj,
                        toolrecord = toolrecord,
                        toolfile = objects,


                    };
                }
            
               
                //var recordentity = toolrecordbll.GetList("").Where(p => p.ToolEquipmentId == keyValue).ToList();
                //List<object> toolrecord = new List<object>();
                //foreach (var entity in recordentity)
                //{
                //    toolrecord.Add(new
                //    {
                //        id = entity.Id,
                //        createuserid = entity.CreateUserId,
                //        createuserdeptcode = entity.CreateUserDeptCode,
                //        createuserorgcode = entity.CreateUserOrgCode,
                //        createdate = entity.CreateDate,
                //        createusername = entity.CreateUserName,
                //        modifydate = entity.ModifyDate,
                //        modifyuserid = entity.ModifyUserId,
                //        modifyusername = entity.ModifyUserName,
                //        nextcheckdate = entity.NextCheckDate,
                //        equipmentname = entity.EquipmentName,
                //        equipmentno = entity.EquipmentNo,
                //        operuserid = entity.OperUserId,
                //        voltagelevel = entity.VoltageLevel,
                //        operuser = entity.OperUser,
                //        appraise = entity.Appraise,
                //        trialvoltage = entity.TrialVoltage,
                //        checkdate = entity.CheckDate,
                //        toolequipmentid = entity.ToolEquipmentId,
                //        specification = entity.Specification,
                //        checkproject = entity.CheckProject
                //    });
                //}


                return new { Code = 0, Count = records, Info = "获取数据成功", data = data };

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }

        /// <summary>
        /// 获取工器具名称
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetToolName([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string tooltype = res.Contains("tooltype") ? dy.data.tooltype : "";
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                if (null == user)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                object obj = toolequipmentBll.GetToolName(tooltype);
                if (obj == null)
                {
                    return new { Code = -1, Count = 0, Info = "没有查询到数据！" };
                }
                else
                {
                    return new { Code = 0, Count = -1, Info = "获取数据成功", data = obj };
                }

            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }



        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="foldername"></param>
        /// <param name="fileList"></param>
        public void UploadifyFile(string descriptionfileid, string contractfileid,string recordid, HttpFileCollection fileList)
        {
            try
            {
                string folderId = "";
                if (fileList.Count > 0)
                {
                    foreach (string key in fileList.AllKeys)
                    {
                        if (key.ToLower().Contains("description"))
                        {
                            folderId = descriptionfileid;
                        }
                        else if (key.ToLower().Contains("contract"))
                        {
                            folderId = contractfileid;
                        }
                        else if (key.ToLower().Contains("record"))
                        {
                            folderId = recordid;
                        }

                        HttpPostedFile file = fileList[key];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\ht\\images\\" + uploadDate;
                        string newFileName = fileGuid + FileEextension;
                        string newFilePath = dir + "\\" + newFileName;
                        //创建文件夹
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
                            fileInfoEntity.FolderId = "ht/images";
                            fileInfoEntity.RecId = folderId; //关联ID
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = "~/Resource/ht/images/" + uploadDate + '/' + newFileName;
                            fileInfoEntity.FileSize = (Math.Round(decimal.Parse(filesize.ToString()) / decimal.Parse("1024"), 2)).ToString();//文件大小（kb）
                            fileInfoEntity.FileExtensions = FileEextension;
                            fileInfoEntity.FileType = FileEextension.Replace(".", "");
                            fileInfoBLL.SaveForm("", fileInfoEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="fileInfoIds"></param>
        public bool DeleteFile(string fileInfoIds)
        {
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
                        // var filePath = ctx.Server.MapPath(entity.FilePath);
                        var filePath = new DataItemDetailBLL().GetItemValue("imgPath") +
                                       entity.FilePath.Replace("~", "");
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

    }

    public class ToolModel
    {
        public string keyvalue { get; set; }
        public ToolequipmentEntity entity { get; set; }
        public string delfileids { get; set; }
    }
}
