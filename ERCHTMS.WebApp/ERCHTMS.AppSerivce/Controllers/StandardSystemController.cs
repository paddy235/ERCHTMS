using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Code;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using ERCHTMS.Busines.StandardSystem;
using ERCHTMS.Entity.StandardSystem;
using ERCHTMS.Busines.PublicInfoManage;
using ERCHTMS.Busines.SystemManage;
using System.Data;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 标准体系
    /// </summary>
    public class StandardSystemController : BaseApiController
    {
        private PostBLL postbll = new PostBLL();
        private StandardsystemBLL standardsystembll = new StandardsystemBLL();
        private StcategoryBLL stcategorybll = new StcategoryBLL();
        private StandardreadrecordBLL standardreadrecordbll = new StandardreadrecordBLL();
        private DepartmentBLL departmentbll = new DepartmentBLL();
        private ElementBLL elementbll = new ElementBLL();
        #region 获取岗位
        [HttpPost]
        public object GetPostList([FromBody]JObject json)
        {

            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            OperatorProvider.AppUserId = dy.userid;
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            
            //获取岗位
            var list = postbll.GetPostForAPP();

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = list.Rows.Count,
                data = list
            };
        }

        #endregion

        #region 获取标准体系列表
        [HttpPost]
        public object GetPageList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            OperatorProvider.AppUserId = dy.userid;
            string standardtype = res.Contains("standardtype") ? dy.data.standardtype : "";
            string encode = res.Contains("encode") ? dy.data.encode : "";
            string station = res.Contains("station") ? dy.data.station : "";
            string keyword = res.Contains("keyword") ? dy.data.keyword : "";
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            Operator user = OperatorProvider.Provider.Current();
            var watch = CommonHelper.TimerStart();
            Pagination pagination = new Pagination();
            pagination.p_kid = "a.ID";
            pagination.p_fields = "a.filename,to_char(a.createdate,'yyyy-mm-dd hh24:mi:ss') createdate,(case  when  c.recid is null then '0' else '1' end) as isnew,a.standardtype ";
            pagination.p_tablename = " hrs_standardsystem a left join hrs_stcategory b on a.categorycode =b.id left join hrs_standardreadrecord c on a.id =c.recid and c.createuserid ='" + user.UserId + "'";
            pagination.conditionJson = "a.standardtype not in ('7','8')";
            pagination.sidx = "isnew,a.createdate";
            pagination.sord = "asc";
            pagination.page = int.Parse(dy.pageindex.ToString());
            pagination.rows = int.Parse(dy.pagesize.ToString());
            if (!user.IsSystem)
            {
                pagination.conditionJson += " and a.createuserorgcode='" + user.OrganizeCode + "'";
            }

            //查询条件
            if (standardtype.Length > 0)
                pagination.conditionJson += " and a.standardtype='" + standardtype + "'";
            if (encode.Length > 0)
                pagination.conditionJson += " and b.id in (select id from hrs_stcategory where encode like '" + encode + "%')";
            if (station.Length > 0)
            {
                string[] PostidList = station.Replace("，", ",").Split(',');
                string forsql = " and (";
                foreach (var item in PostidList)
                {
                    forsql += "stationid like '%" + item.ToString() + "%' or";
                }
                if (forsql.Length > 6)
                {
                    forsql = forsql.Substring(0, forsql.Length - 2);
                }
                forsql += ")";
                pagination.conditionJson += forsql;
            }
            if (keyword.Length > 0)
                pagination.conditionJson += " and (stationname like '%" + keyword + "%' or filename like '%" + keyword + "%' or b.name like '%" + keyword + "%' )";

            //获取列表
            var list = standardsystembll.GetPageList(pagination, "");

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = pagination.records,
                data = list
            };
        }
        #endregion

        #region 获取各种标准体系分类
        [HttpPost]
        public object GetCatoryList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            OperatorProvider.AppUserId = dy.userid;
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            //获取岗位
            var data = stcategorybll.GetList("").Where(t => t.TYPECODE == dy.data.standardtype).OrderBy(t => t.ENCODE).ToList();

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = data.Count(),
                data = data.Select(g => new {
                    id = g.ID,
                    parentid = g.PARENTID,
                    name = g.NAME,
                    encode = g.ENCODE
                }).ToList()
            };
            
        }
        #endregion

        #region 获取标准体系详情
        [HttpPost]
        public object GetStandardDetail([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userId = dy.userid;
                string keyValue = dy.data.keyvalue;
                OperatorProvider.AppUserId = userId;  //设置当前用户
                Operator currUser = OperatorProvider.Provider.Current();
                //阅览频次加1
                var data = standardsystembll.GetEntity(keyValue);
                data.CONSULTNUM += 1;
                standardsystembll.SaveForm(keyValue, data);
                //添加阅览详细记录
                var standardreadrecordentity = standardreadrecordbll.GetList("").Where(t => t.RecId == keyValue && t.CreateUserId == currUser.UserId).FirstOrDefault();
                standardreadrecordentity = standardreadrecordentity == null ? new StandardreadrecordEntity() : standardreadrecordentity;
                standardreadrecordentity.RecId = keyValue;
                standardreadrecordbll.SaveForm(standardreadrecordentity.ID, standardreadrecordentity);
                var standardsystemEntity = standardsystembll.GetEntity(keyValue);//获取标准实体
                if (!string.IsNullOrEmpty(standardsystemEntity.CATEGORYCODE))
                {
                    var catory = stcategorybll.GetEntity(standardsystemEntity.CATEGORYCODE);
                    standardsystemEntity.CATEGORYNAME = string.IsNullOrEmpty(catory.NAME) ? "" : catory.NAME;
                }
                standardsystemEntity.CREATEUSERDEPTNAME = departmentbll.GetEntityByCode(standardsystemEntity.CREATEUSERDEPTCODE).FullName;
                var files = new FileInfoBLL().GetFiles(keyValue);
                string webUrl = new DataItemDetailBLL().GetItemValue("imgUrl");
                foreach (DataRow dr in files.Rows)
                {
                    dr["filepath"] = dr["filepath"].ToString().Replace("~/", webUrl + "/");
                }
                return new
                {
                    Code = 0,
                    Count = 1,
                    Info = "获取数据成功",
                    data = new
                    {
                        entity = new
                        {
                            id = standardsystemEntity.ID,
                            filename = standardsystemEntity.FILENAME,
                            categorycode = standardsystemEntity.CATEGORYCODE,
                            categoryname = standardsystemEntity.CATEGORYNAME,
                            stationid = standardsystemEntity.STATIONID,
                            stationname = standardsystemEntity.STATIONNAME,
                            relevantelementname = standardsystemEntity.RELEVANTELEMENTNAME,
                            relevantelementid = standardsystemEntity.RELEVANTELEMENTID,
                            carrydate = standardsystemEntity.CARRYDATE,
                            createdate = standardsystemEntity.CREATEDATE,
                            createuserdeptname = standardsystemEntity.CREATEUSERDEPTNAME,
                            dispatchcode = standardsystemEntity.DISPATCHCODE,
                            publishdept = standardsystemEntity.PUBLISHDEPT
                        },
                        files = files
                    }
                };
            }
            catch (Exception ex)
            {
                return new { Code = -1, Count = 0, Info = ex.Message };
            }

        }
        #endregion

        #region 获取未读的标准总数
        [HttpPost]
        public object GetNonReadStandardNum([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            OperatorProvider.AppUserId = dy.userid;
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            var list = standardsystembll.GetStandardCount();
            return new
            {
                code = 0,
                info = "获取数据成功",
                count = 1,
                data = list
            };

        }
        #endregion

        #region 获取对应元素
        [HttpPost]
        public object GetElementList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");

            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            OperatorProvider.AppUserId = dy.userid;
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }

            //获取岗位
            var data = elementbll.GetList("").OrderBy(t => t.ENCODE).ToList();

            return new
            {
                code = 0,
                info = "获取数据成功",
                count = data.Count(),
                data = data.Select(g => new
                {
                    id = g.ID,
                    parentid = g.PARENTID,
                    name = g.NAME,
                    encode = g.ENCODE
                })
            };

        }
        #endregion
    }
}
