using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.AppSerivce.Controllers;
using ERCHTMS.Busines.HseToolManage;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.Code;
using ERCHTMS.Entity.HseToolMange;
using ERCHTMS.Entity.PublicInfoManage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ERCHTMS.Busines.HseToolManage;
using ERCHTMS.Busines.BaseManage;

namespace ERCHTMS.AppSerivce
{
    public class HseToolController : BaseApiController
    {
        private DataItemDetailBLL dataitemdetailbll = new DataItemDetailBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();
        private HseObserveBLL hseobservebll = new HseObserveBLL();
        private HseObserveNormBLL hseobserveNormbll = new HseObserveNormBLL();

        private FileInfoBLL fileInfoBLL = new FileInfoBLL();
        public HttpContext ctx { get { return HttpContext.Current; } }
        #region HSE安全观察

        #region 查询

        /// <summary>
        /// 根据用户获取未提交的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetHseObserveList([FromBody]JObject json)
        {
            try
            {
                var watch = CommonHelper.TimerStart();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                //全部0 / 我的1
                string Type = res.Contains("Type") ? dy.data.Type : "";
                //全部0 / 审核1
                string isspecial = res.Contains("isspecial") ? dy.data.isspecial : "";
                //关闭状态
                string obstate = res.Contains("obstate") ? dy.data.obstate : "";

                //开始日期
                string sTime = res.Contains("stime") ? dy.data.stime : "";
                //结束日期
                string eTime = res.Contains("etime") ? dy.data.etime : "";

                //观察者
                string lookuser = res.Contains("lookuser") ? dy.data.lookuser : "";

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
                pagination.p_fields = @"CREATEUSERID,CREATEUSERDEPTCODE,CREATEUSERORGCODE,DEPARTMENTCODE AS DEPTCODE,CREATEDATE,CREATEUSERNAME,MODIFYDATE,MODIFYUSERID,MODIFYUSERNAME,OBSERVEUSER,OBSERVEUSERID,DEPARTMENT,DEPARTMENTID,TASK,AREA,OBSERVEDATE,CONTENT,OBSERVETYPE,DESCRIBE,ISMODIFY,MEASURES,OBSERVEACTION,OBSERVELEVEL,OBSERVESTATE,OBSERVERESULT,CREATEUSERDEPT";
                pagination.p_tablename = @"HSE_SECURITYOBSERVE  ";
                pagination.sidx = "createdate";
                pagination.sord = "desc";
                pagination.conditionJson = "1=1";
                //时间范围
                if (!string.IsNullOrEmpty(sTime) || !string.IsNullOrEmpty(eTime))
                {
                    if (string.IsNullOrEmpty(sTime))
                    {
                        sTime = "1899-01-01";
                    }
                    else
                    {
                        sTime = Convert.ToDateTime(sTime).ToString("yyyy-MM-dd");
                    }
                    if (string.IsNullOrEmpty(eTime))
                    {
                        eTime = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                    eTime = (Convert.ToDateTime(eTime).AddDays(1)).ToString("yyyy-MM-dd");
                    pagination.conditionJson += string.Format(" and CREATEDATE between to_date('{0}','yyyy-MM-dd') and  to_date('{1}','yyyy-MM-dd')", sTime, eTime);
                }
                if (!string.IsNullOrEmpty(lookuser))
                {
                    pagination.conditionJson += string.Format(" and  CREATEUSERNAME='{0}'", lookuser);
                }
                if (!string.IsNullOrEmpty(obstate))
                {
                    pagination.conditionJson += string.Format(" and  OBSERVESTATE='{0}'", obstate);
                }

                if (!string.IsNullOrEmpty(Type))
                {
                    if (Type == "0")
                    {
                        pagination.conditionJson += string.Format(" and  OBSERVESTATE!='{0}'", "未提交");

                    }
                    else
                    if (Type == "1")
                    {
                        pagination.conditionJson += string.Format(" and  CREATEUSERID='{0}'", userId);
                    }
                    if (!string.IsNullOrEmpty(isspecial))
                    {
                        //审核用户
                        if (isspecial == "1")
                        {
                            if (curUser.RoleName.Contains("公司级用户"))
                            {
                                pagination.conditionJson += string.Format(" and  OBSERVESTATE='{0}' and CREATEUSERID!='{1}' ", "待整改关闭", userId);

                            }
                            else
                            {
                                pagination.conditionJson += string.Format(" and  OBSERVESTATE='{0}' and DEPARTMENTID='{1}' and CREATEUSERID!='{2}'", "待整改关闭", curUser.DeptId, userId);

                            }
                        }
                    }

                }
                else
                {
                    pagination.conditionJson += string.Format(" and  OBSERVESTATE!='{0}'", "未提交");

                }
                var rewarddata = hseobservebll.GetPageList(pagination, null);
                var data = new
                {
                    rows = rewarddata,
                    total = pagination.total,
                    page = pagination.page,
                    records = pagination.records,
                    costtime = CommonHelper.TimerEnd(watch)
                };
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
        public object GetHseObserveInfo([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string dataId = dy.data;
                HseObserveEntity entity = hseobservebll.GetEntity(dataId);

                //获取相关附件
                var files = new FileInfoBLL().GetFiles(dataId);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                var result = new
                {
                    entity = entity,
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
        /// 获取观察类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsType([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string type = dy.data.type;
                var data = dataitemdetailbll.GetDataItemListByItemCode("'" + type + "'").ToList();
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }

        /// <summary>
        /// 获取观察类别
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetObsTypeNew([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                var data = hseobserveNormbll.GetList();
                return new { code = 0, count = data.Count, info = "获取数据成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        /// <summary>
        /// 获取部门是否厂级
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetDeptIsOrg([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string deptid = dy.data;
                var data = departmentBLL.GetEntity(deptid);
                return new { code = 0, info = "获取数据成功", data = new { IsOrg = data.IsOrg == 1 } };
            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }
        }
        #endregion

        #region 数据操作

        /// <summary>
        /// 新增修改
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveModifyRecord()
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
                string entitystr = JsonConvert.SerializeObject(dy.data.entity);
                var entity = JsonConvert.DeserializeObject<HseObserveEntity>(entitystr);
                //处理附件
                if (!string.IsNullOrEmpty(deleteids))
                {
                    DeleteFile(deleteids);
                }
                if (string.IsNullOrEmpty(entity.Id))
                {
                    entity.Id = Guid.NewGuid().ToString();
                    entity.CREATEDATE = DateTime.Now;
                    entity.CREATEUSERDEPT = user.DeptName;
                    entity.CREATEUSERID = user.UserId;
                    entity.CREATEUSERNAME = user.UserName;
                    entity.CREATEUSERDEPTCODE = user.DeptCode;
                    entity.CREATEUSERORGCODE = user.OrganizeCode;
                    entity.MODIFYDATE = DateTime.Now;
                    entity.MODIFYUSERID = user.UserId;
                    entity.MODIFYUSERNAME = user.UserName;
                    hseobservebll.SaveForm("", entity);
                }
                else
                {
                    entity.MODIFYDATE = DateTime.Now;
                    entity.MODIFYUSERID = user.UserId;
                    entity.MODIFYUSERNAME = user.UserName;
                    hseobservebll.SaveForm(entity.Id, entity);
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
                        string dir = new DataItemDetailBLL().GetItemValue("imgPath") + "\\Resource\\HseObserve";
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
                            fileInfoEntity.FilePath = "~/Resource/HseObserve/" + newFileName;
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
        ///关闭观察卡
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object SaveSuccessRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string dataId = dy.data.keyvalue;
                string content = dy.data.content;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                HseObserveEntity entity = hseobservebll.GetEntity(dataId);
                entity.ObserveResult = content;
                entity.ObserveState = "已关闭";
                entity.MODIFYDATE = DateTime.Now;
                entity.MODIFYUSERID = user.UserId;
                entity.MODIFYUSERNAME = user.UserName;
                hseobservebll.SaveForm(entity.Id, entity);
                return new { code = 0, count = 0, info = "操作成功" };

            }
            catch (Exception ex)
            {
                return new { code = -1, count = 0, info = ex.Message };
            }


        }

        /// <summary>
        ///删除观察卡
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object delObserveRecord([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string dataId = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                var user = ERCHTMS.Code.OperatorProvider.Provider.Current();
                hseobservebll.RemoveForm(dataId);

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

        #endregion

        #endregion



        #region 自我评估
        /// <summary>
        /// 新增或修改自我评价
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveSelfEvaluate()
        {
            try
            {
                var bll = new SelfEvaluateBLL();
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                string dataJson = JsonConvert.SerializeObject(dy.data);
                SelfEvaluateEntity entity = JsonConvert.DeserializeObject<SelfEvaluateEntity>(dataJson);
                if (string.IsNullOrEmpty(entity.Id)) //新增自我评估
                {
                    entity.Id = Guid.NewGuid().ToString();
                }
                entity.CreateUser = curUser.UserName;
                entity.CreateUserId = curUser.UserId;
                entity.DeptCode = curUser.DeptCode;
                entity.DeptId = curUser.DeptId;
                entity.DeptName = curUser.DeptName;


                if (string.IsNullOrEmpty(entity.A.Id)) //新增自我评估
                {
                    entity.A.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.B.Id)) //新增自我评估
                {
                    entity.B.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.C.Id)) //新增自我评估
                {
                    entity.C.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.D.Id)) //新增自我评估
                {
                    entity.D.Id = Guid.NewGuid().ToString();
                }
                if (string.IsNullOrEmpty(entity.E.Id)) //新增自我评估
                {
                    entity.E.Id = Guid.NewGuid().ToString();
                }
                entity.A.EvaId = entity.Id;
                entity.B.EvaId = entity.Id;
                entity.C.EvaId = entity.Id;
                entity.D.EvaId = entity.Id;
                entity.E.EvaId = entity.Id;
                if (entity.IsSubmit == "1") entity.IsFill = "1";
                bll.SaveForm(entity);
                return new { code = 0, count = 0, info = "获取成功", data = 0 };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }

        /// <summary>
        /// 自我评估列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetSelfEvaluateList([FromBody]JObject json)
        {
            try
            {
                var bll = new SelfEvaluateBLL();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                string type = dy.data.type;
                string name = dy.data.name;
                string userid = dy.userid;
                string keyword = dy.data.keyword;
                string year = dy.data.year;
                string deptcode = dy.data.deptcode;
                string state = dy.data.state;
                string month = dy.data.month;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                var list = new List<SelfEvaluateEntity>();
                if (string.IsNullOrEmpty(deptcode))
                {

                    list = bll.GetList("", curUser.DeptCode, keyword, year, month).ToList();
                    deptcode = curUser.DeptCode;
                }
                else
                {
                    list = bll.GetList("", deptcode, keyword, year, month).ToList();
                }
                if (type == "my")
                {
                    list = list.Where(x => x.CreateUserId == curUser.UserId).ToList();
                }
                else
                {
                    if (state != "已填报")
                    {
                        var allUser = new UserBLL().GetList().Where(x => x.IsPresence == "1");
                        List<string> submituserids = list.Where(x => x.IsSubmit == "1").Select(x => x.CreateUserId).ToList();//该部门下已提交人的人的Id
                        List<string> submitUserNames = allUser.Where(p => submituserids.Contains(p.UserId) && p.IsPresence == "1").Select(p => p.RealName).ToList();//已提交的人
                        List<Entity.BaseManage.UserEntity> notSubmitUserNames = allUser.Where(p => p.DepartmentCode != null && p.DepartmentCode.StartsWith(deptcode) && !submituserids.Contains(p.UserId)).ToList();//未提交的人
                        if (state == "未填报")
                        {
                            list.Clear();
                        }
                        var yearNum = Convert.ToInt32(year);
                        var monthNum = Convert.ToInt32(month);
                        var dayTime = new DateTime(yearNum, monthNum, 1);
                        notSubmitUserNames.ForEach(x =>
                        {
                            var model = new SelfEvaluateEntity();
                            model.CreateUser = x.RealName;
                            model.CreateDate = dayTime;
                            model.DeptId = x.DepartmentId;
                            model.DeptCode = x.DepartmentCode;
                            model.IsSubmit = "1";
                            list.Add(model);
                        });
                    }
                    list = list.Where(x => x.IsSubmit == "1").ToList();
                }

                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(x => x.CreateUser.Contains(name)).ToList();
                }
                int count = list.Count();
                list = list.Skip((page - 1) * rows).Take(rows).OrderBy(p => p.IsFill).OrderByDescending(x => x.IsSubmit).ThenByDescending(p => p.CreateDate).ToList();
                var deptbll = new DepartmentBLL();
                var allDept = deptbll.GetList();
                list.ForEach(x =>
                {
                    var dept = string.Empty;
                    var go = true;
                    var getDept = allDept.FirstOrDefault(p => p.DepartmentId == x.DeptId);
                    var parDeptId = getDept.ParentId;
                    if (getDept.Nature == "班组" || getDept.Nature == "专业")
                    {
                        dept = getDept.FullName;
                        while (go)
                        {
                            var parDept = deptbll.GetEntity(parDeptId);
                            if (parDept == null)
                            {
                                go = false;
                                break;
                            }
                            if (parDept.Nature == "部门")
                            {
                                if (string.IsNullOrEmpty(dept))
                                {
                                    dept = parDept.FullName;
                                }
                                else
                                {
                                    dept = parDept.FullName + "/" + dept;
                                }
                                go = false;
                            }
                            else
                            {
                                parDeptId = parDept.ParentId;
                            }
                        }
                    }
                    else
                    {
                        dept = getDept.FullName;
                    }
                    x.DeptName = dept;
                });
                return new { code = 0, count = count, info = "获取成功", data = list };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }

        /// <summary>
        /// 自我评估台账列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetSelfEvaluateMonthList([FromBody]JObject json)
        {
            try
            {
                var bll = new SelfEvaluateBLL();
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                int page = Convert.ToInt32(dy.data.pageNum), rows = Convert.ToInt32(dy.data.pageSize);
                string keyword = dy.data.keyword;
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                var list = bll.GetList("", curUser.DeptCode, keyword).ToList();
                var nlist = new List<MonthEvaluate>();
                var ylist = list.GroupBy(x => x.Year).Select(x => x.FirstOrDefault().Year).OrderByDescending(x => x); //根据年分分组
                var userCount = new UserBLL().GetList().Count(p => p.DepartmentCode.StartsWith(curUser.DeptCode) && p.IsPresence=="1");
                foreach (string year in ylist)
                {
                    var mlist = list.Where(x => x.Year == year).GroupBy(x => x.Month).Select(x => x.FirstOrDefault().Month).OrderByDescending(x => x);//查询当年数据根据月份分组
                    foreach (string month in mlist)
                    {
                        var obj = new MonthEvaluate();
                        obj.name = year + "年" + month + "月员工HSE自我评估";
                        obj.count1 = list.Where(x => x.Year == year && x.Month == month && x.IsFill == "1").GroupBy(x => x.CreateUserId).Count();
                        obj.count2 = ((userCount - obj.count1) < 0 ? 0 : (userCount - obj.count1));
                        obj.year = year;
                        obj.month = month;
                        var summary = bll.GetSummary(year, month, curUser.DeptId);
                        obj.content = summary == null ? "" : summary.Content;
                        nlist.Add(obj);
                    }
                }
                return new { code = 0, count = 0, info = "获取成功", data = nlist };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        /// <summary>
        /// 获取自我评估详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetSelfEvaluateInfo([FromBody]JObject json)
        {
            try
            {
                var bll = new SelfEvaluateBLL();
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                var entity = bll.GetEntity(dy.data);
                return new { code = 0, count = 0, info = "获取成功", data = entity };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }

        /// <summary>
        /// 新增小结
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object SaveSummary()
        {
            try
            {
                var bll = new SelfEvaluateBLL();
                string res = HttpContext.Current.Request["json"];
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!", data = new object() };
                }
                EvaluateGroupSummaryEntity entity = JsonConvert.DeserializeObject<EvaluateGroupSummaryEntity>(JsonConvert.SerializeObject(dy.data));
                if (string.IsNullOrEmpty(entity.Id)) entity.Id = Guid.NewGuid().ToString();
                bll.SaveSummary(entity);
                return new { code = 0, count = 0, info = "获取成功", data = 0 };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }

        /// <summary>
        /// 评估选项复选框数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public object GetCheckData()
        {
            try
            {
                var entity = new DataEntity();
                entity.Danger = new List<string>() { "垂直交叉", "落物可能", "飞溅作业", "粉尘", "烟尘", "噪音", "振动", "高处/临边", "脚手架", "夜间", "带电设备", "强酸", "强碱", "受限空间", "高温", "低温", "特殊气体", "照明不足", "无" };
                entity.PPe = new List<string>() { "安全帽", "防护眼镜", "防护眼罩", "防护面罩", "防尘口罩", "焊接口罩", "防尘面罩", "耳塞", "耳罩", "安全带", "防坠器", "锚点", "手电", "对讲机", "绝缘手套", "绝缘靴", "绝缘服", "防护眼镜", "防护面罩", "防护服", "气体检测仪", "隔热手套", "隔热鞋", "隔热服", "棉手套", "皮手套", "线手套", "无" };
                entity.Traffic = new List<string>() { "超过限制速度驾驶", "饮酒后驾驶", "驾驶中打电话、疲劳、精力分散", "逆行驾车、开斗气车", "驾车横行掉头、违规停车", "搭顺风车", "上、下车未注意头上、脚下安全", "私自允许本单位人员乘坐单位车辆", "非停靠站点拦截车辆", "骑车未佩戴安全头盔", "行走、驾驶抢道、闯红灯" };
                entity.Electricity = new List<string>() { "不合格的家电设备", "破损的家电设备", "电气设备安全装置确实", "人离开未切断电源", "搭顺风车" };
                entity.Fire = new List<string>() { "电器超载过热", "气瓶、管道老化", "错误使用微波炉/烤炉/电饭煲/电熨斗", "消防器材失效或缺失", "堆积大量的可燃易燃材料未处理" };
                entity.Power = new List<string>() { "地板水渣、油污", "地面或通道有杂物", "超出体力搬运", "操作遮挡视线", "无防护推窗", "阳台外有易坠重物", "用椅子高处取物", "弯腰取物" };
                entity.Other = new List<string>() { "机场闹事", "乘坐扶梯（滚梯）注意力不集中", "非游泳区游泳", "宿舍存放危险品", "宿舍私自容留外人住宿", "因声音过大影响他人而产生矛盾、纠纷", "遭遇诈骗", "违反当地法规、风俗", "与社会人员发生纠纷", "喝酒误事、酒后闹事", "聚众闹事、聚众赌博、吸毒", "破坏草坪、花卉、树木", "陷入拥挤、踩踏危险境地" };
                return new { code = 0, count = 0, info = "获取成功", data = entity };
            }
            catch (Exception ex)
            {

                return new { code = -1, count = 0, info = "获取数据失败：" + ex.Message, data = new object() };
            }

        }
        /// <summary>
        /// 获取小结
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetSummary([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string id = dy.data.id;
                string userid = dy.userid;
                string year = dy.data.year;
                string month = dy.data.month;
                string deptId = dy.data.deptid;
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, info = "请求失败,请登录!", data = new object() };
                }
                var bll = new SelfEvaluateBLL();
                EvaluateGroupSummaryEntity data = new EvaluateGroupSummaryEntity();
                if (string.IsNullOrWhiteSpace(id))
                    data = bll.GetSummary(year, month, deptId);
                else
                    data = bll.GetSummaryById(id);

                return new { code = 0, info = "获取成功", data = data };
            }
            catch (Exception ex)
            {
                return new { code = -1, info = "获取数据失败：" + ex.Message, data = new object() };
            }
        }

        #endregion
    }
    public class MonthEvaluate
    {
        public string name { get; set; }
        public int count1 { get; set; }
        public int count2 { get; set; }
        public string content { get; set; }
        public string year { get; set; }
        public string month { get; set; }
    }

    public class DataEntity
    {
        public List<string> Danger { get; set; }

        public List<string> PPe { get; set; }
        public List<string> Traffic { get; set; }
        public List<string> Electricity { get; set; }
        public List<string> Fire { get; set; }

        public List<string> Power { get; set; }
        public List<string> Other { get; set; }
    }
}