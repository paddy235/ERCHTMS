using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ERCHTMS.AppSerivce.Model;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.EngineeringManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.EngineeringManage;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class EngineeringController : BaseApiController
    {
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private EngineeringSettingBLL eSettingBll = new EngineeringSettingBLL();
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private PerilEngineeringBLL perilengineeringbll = new PerilEngineeringBLL();
        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        public HttpContext hcontent { get { return HttpContext.Current; } }

        #region 危大工程
        #region 获取工程类别
        /// <summary>
        /// 获取工程类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getProgrammeTypeList()
        {
            try
            {
                var data = eSettingBll.GetList("");
                return new { Code = 0, Count = data.Count(), Info = "获取数据成功", data = data.Select(x => new { programmename = x.ProgrammeCategory, programmeid = x.Id }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 获取工程进展情况
        /// <summary>
        /// 获取工程进展情况
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
      [HttpPost]
        public object getProgrammeCaseList()
        {
            try
            {
                var itemlist = dataitemdetailbll.GetDataItemListByItemCode("'EvolveSituation'");
                return new { code = 0, info = "获取数据成功", count = itemlist.Count(), data = itemlist.Select(x => new { programmecasevalue = x.ItemValue, programmecasename = x.ItemName }) };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 根据工程类别获取风险点等内容
        /// <summary>
        /// 根据工程类别获取风险点等内容
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getCodeList([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string prnameid = dy.data.programmeid;
                var result = eSettingBll.GetEntity(prnameid);
                return new { Code = 0, Count = 0, Info = "获取数据成功", data = new { engineeringspot = result.ProgrammeRisk, taskcontent = result.ProgrammeContent } };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 新增/修改危大工程记录
        /// <summary>
        /// 新增/修改危大工程记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddOrUpdateEngineeringRecord()
        {
            try
            {
                string res = hcontent.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                PerilEngineeringEntity entity = new PerilEngineeringEntity();
                string engineeringid = res.Contains("engineeringid") ? dy.data.engineeringid : "";  //主键
                //如果engineeringid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(engineeringid))
                {
                    entity = perilengineeringbll.GetEntity(engineeringid);
                }
                entity.EngineeringName = dy.data.engineeringname;//工程名称
                entity.EngineeringType = dy.data.engineeringtype;//工程类别id
                entity.EngineeringSpot = dy.data.engineeringspot;//工程风险点
                entity.EStartTime = string.IsNullOrEmpty(dy.data.estarttime) ? "" : Convert.ToDateTime(dy.data.estarttime);//开始时间
                entity.EFinishTime =string.IsNullOrEmpty(dy.data.efinishtime) ? "" : Convert.ToDateTime(dy.data.efinishtime);//结束时间
                entity.BelongDeptId = dy.data.belongdeptid;//所属单位id
                DepartmentEntity dept = departmentBLL.GetEntity(dy.data.belongdeptid);
                if (dept.Nature.Contains("承包商") || dept.Nature.Contains("分包商"))
                    entity.UnitType = "2";//外包单位
                else
                    entity.UnitType = "1";//电厂内部
                entity.BelongDeptName = dy.data.belongdeptname;//所属单位名称
                entity.ExaminePerson = dy.data.examineperson;//审核人
                entity.ExamineTime =string.IsNullOrEmpty(dy.data.examinetime) ? "" : Convert.ToDateTime(dy.data.examinetime);//审核时间

                entity.TaskTime = string.IsNullOrEmpty(dy.data.tasktime) ? "" : Convert.ToDateTime(dy.data.tasktime);//交底时间
                entity.TaskPerson = dy.data.taskperson;//交底人
                entity.TaskContent = dy.data.taskcontent;//交底内容
                entity.EvolveCase = dy.data.evolvecase;//进展情况
                entity.Remark = dy.data.remark;//备注
                entity.WriteDate = string.IsNullOrEmpty(dy.data.writedate) ? "" : Convert.ToDateTime(dy.data.writedate);//编制时间
                entity.WriteUserName = dy.data.writeusername;//编制人
                HttpFileCollection files = hcontent.Request.Files;
                var cfilesid = Guid.NewGuid().ToString();
                var tfilesid = Guid.NewGuid().ToString();
                string[] fileids = new string[2];
                fileids[0] = cfilesid;
                fileids[1] = tfilesid;
                entity.ConstructFiles = cfilesid;//施工方案文件id
                entity.TaskFiles = tfilesid;//交底文件id
                //上传文件
                UploadifyFile(fileids, "Engineering", files);

                perilengineeringbll.SaveForm(entity.Id, entity);

            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region  文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="Filedata"></param>
        public void UploadifyFile(string[] folderIds, string foldername, HttpFileCollection fileList)
        {
            try
            {
                if (fileList.Count > 0)
                {
                    for (int i = 0; i < fileList.AllKeys.Length; i++)
                    {
                        HttpPostedFile file = fileList[i];
                        //获取文件完整文件名(包含绝对路径)
                        //文件存放路径格式：/Resource/ResourceFile/{userId}{data}/{guid}.{后缀名}
                        string userId = OperatorProvider.Provider.Current().UserId;
                        string fileGuid = Guid.NewGuid().ToString();
                        long filesize = file.ContentLength;
                        string FileEextension = Path.GetExtension(file.FileName);
                        string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                        string virtualPath = string.Format("~/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string virtualPath1 = string.Format("/Resource/ht/images/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                        string fullFileName = dataitemdetailbll.GetItemValue("imgPath") + virtualPath1;
                        //创建文件夹
                        string path = Path.GetDirectoryName(fullFileName);
                        Directory.CreateDirectory(path);
                        FileInfoEntity fileInfoEntity = new FileInfoEntity();
                        if (!System.IO.File.Exists(fullFileName))
                        {
                            //保存文件
                            file.SaveAs(fullFileName);
                            //文件信息写入数据库

                            fileInfoEntity.Create();
                            fileInfoEntity.FileId = fileGuid;
                            if (file.FileName.Contains("sg"))
                            {
                                fileInfoEntity.RecId = folderIds[0]; //关联ID
                            }
                            if (file.FileName.Contains("jd"))
                            {
                                fileInfoEntity.RecId = folderIds[1]; //关联ID
                            }
                            fileInfoEntity.FolderId = "ht/images";
                            fileInfoEntity.FileName = file.FileName;
                            fileInfoEntity.FilePath = virtualPath;
                            fileInfoEntity.FileSize = filesize.ToString();
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
        #endregion

        #region 获取危大工程详情
        /// <summary>
        /// 获取危大工程详情
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getEngineeringDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string engineeringid = dy.data.engineeringid;
                PerilEngineeringEntity entity = perilengineeringbll.GetEntity(engineeringid);
                DataTable cdt = fileInfoBLL.GetFiles(entity.ConstructFiles);
                IList<Photo> cfiles = new List<Photo>(); //方案文件
                foreach (DataRow item in cdt.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    cfiles.Add(p);
                }
                DataTable tfile = fileInfoBLL.GetFiles(entity.TaskFiles);
                IList<Photo> tfiles = new List<Photo>(); //交底文件
                foreach (DataRow item in tfile.Rows)
                {
                    Photo p = new Photo();
                    p.id = item[0].ToString();
                    p.filename = item[1].ToString();
                    p.fileurl = dataitemdetailbll.GetItemValue("imgUrl") + item[2].ToString().Substring(1);
                    tfiles.Add(p);
                }
                return new
                {
                    Code = 0,
                    Count = -1,
                    Info = "获取数据成功",
                    data = new { engineeringid = entity.Id, engineeringname = entity.EngineeringName, engineeringtype = getName(entity.EngineeringType), engineeringspot = entity.EngineeringSpot, estarttime = string.IsNullOrEmpty(entity.EStartTime.ToString()) ? "" : Convert.ToDateTime(entity.EStartTime).ToString("yyyy-MM-dd"), EFinishTime = string.IsNullOrEmpty(entity.EFinishTime.ToString()) ? "" : Convert.ToDateTime(entity.EFinishTime).ToString("yyyy-MM-dd"), belongdeptname = entity.BelongDeptName, examineperson = entity.ExaminePerson, ExamineTime = string.IsNullOrEmpty(entity.ExamineTime.ToString()) ? "" : Convert.ToDateTime(entity.ExamineTime).ToString("yyyy-MM-dd"), taskperson = entity.TaskPerson, TaskTime = string.IsNullOrEmpty(entity.TaskTime.ToString()) ? "" : Convert.ToDateTime(entity.TaskTime).ToString("yyyy-MM-dd"), taskcontent = entity.TaskContent, evolvecase = entity.EvolveCase, remark = entity.Remark, writedate = string.IsNullOrEmpty(entity.WriteDate.ToString()) ? "" : Convert.ToDateTime(entity.WriteDate).ToString("yyyy-MM-dd"), writeusername = entity.WriteUserName, cfiles = cfiles, tfiles = tfiles }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion

        #region 工程类别转换
        public string getName(string keyvalue)
        {
            var cName = eSettingBll.GetEntity(keyvalue).ProgrammeCategory;
            return cName;
        }
        #endregion

        #region 获取危大工程列表
        /// <summary>
        /// 根据查询条件获取数据列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object getEngineeringList([FromBody]JObject json)
        {

            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator user = OperatorProvider.Provider.Current();
                string engineeringtype = dy.data.engineeringtype;//工程类别
                string engineeringname = dy.data.engineeringname;//工程名称
                string belongdeptcode = dy.data.belongdeptcode;//所属单位code
                string starttime = dy.data.starttime;//开始时间
                string endtime = dy.data.endtime;//结束时间
                string casetype = dy.data.casetype;//类型(1:正在施工,2:未施工,3:已完工)

                string strwhere = "where 1=1";
                if (!string.IsNullOrEmpty(engineeringtype))
                {
                    strwhere += " and engineeringtype='" + engineeringtype + "'";
                }
                if (!string.IsNullOrEmpty(engineeringname))
                {
                    strwhere += " and engineeringname like '%" + engineeringname + "%'";
                }
                if (!string.IsNullOrEmpty(belongdeptcode))
                {
                    strwhere += string.Format(" and  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", belongdeptcode);
                }
                else
                {
                    if (user.RoleName.Contains("承包商") || user.RoleName.Contains("分包商"))
                    {
                        strwhere += string.Format(" and  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.DeptCode);
                    }
                    else
                    {
                        strwhere += string.Format(" and  belongdeptid  in (select departmentid from base_department where encode like '{0}%' union select ORGANIZEID from BASE_ORGANIZE where encode like '{0}%')", user.OrganizeCode);
                    }
                }
                if (!string.IsNullOrEmpty(starttime))
                {
                    strwhere += string.Format(" and  EStartTime>=to_date('{0}','yyyy-mm-dd')", starttime);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    strwhere += string.Format(" and EFinishTime<=to_date('{0}','yyyy-mm-dd')", endtime);
                }
                if (casetype == "1")//正在施工
                {
                    strwhere += " and EvolveCase='正在施工'";
                }
                if (casetype == "2")//未施工
                {
                    strwhere += " and EvolveCase='未施工'";
                }
                if (casetype == "3")//已完成
                {
                    strwhere += " and EvolveCase='已完工'";
                }
                string sql = string.Format(@"select a.id as engineeringid,programmecategory,engineeringname,belongdeptname from bis_perilengineering a left join bis_engineeringsetting b on a.engineeringtype=b.id {0} order by a.createdate desc", strwhere);
                DataTable dt = perilengineeringbll.GetPerilEngineeringList(sql);
                return new
                {
                    Code = 0,
                    Count = dt.Rows.Count,
                    Info = "获取数据成功",
                    data = dt
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }
        }
        #endregion
        #endregion

        #region 危大工程类别设置

        #region 获取危大工程设置类别列表
        /// <summary>
        /// 获取危大工程设置类别列表
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDailyUseRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string sql = string.Format(@"select id as programmeid,programmecategory,programmerisk from bis_engineeringsetting order by createdate desc ");
                DataTable dt = perilengineeringbll.GetPerilEngineeringList(sql);
                return new { code = 0, info = "获取数据成功", count = dt.Rows.Count, data = dt };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = ex.Message, count = 0 };
            }
        }
        #endregion

        #region 新增/修改危大工程类别设置
        /// <summary>
        /// 新增/修改危大工程类别设置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object AddOrUpdateSettingRecord()
        {
            try
            {
                string res = hcontent.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid; //当前用户ID 
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                EngineeringSettingEntity entity = new EngineeringSettingEntity();
                string esettingid = res.Contains("esettingid") ? dy.data.esettingid : "";  //主键
                //如果esettingid 不为空,则是编辑状态
                if (!string.IsNullOrEmpty(esettingid))
                {
                    entity = eSettingBll.GetEntity(esettingid);
                }
                entity.ProgrammeCategory = dy.data.programmecategory;//工程类别名称
                entity.ProgrammeDescribe = dy.data.programmedescribe;//工程描述
                entity.ProgrammeRisk = dy.data.programmerisk;//工程风险点
                entity.ProgrammeContent = dy.data.programmecontent;//交底内容
                eSettingBll.SaveForm(entity.Id, entity);
            }
            catch (Exception)
            {
                return new { code = -1, count = 0, info = "保存失败" };
            }

            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion
        #endregion
    }
}