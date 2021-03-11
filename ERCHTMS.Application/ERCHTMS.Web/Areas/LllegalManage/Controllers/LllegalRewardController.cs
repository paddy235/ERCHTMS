using ERCHTMS.Entity.LllegalManage;
using ERCHTMS.Busines.LllegalManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using System.Linq;
using ERCHTMS.Code;
using System;
using ERCHTMS.Busines.HiddenTroubleManage;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ERCHTMS.Web.Areas.LllegalManage.Controllers
{
    /// <summary>
    /// 描 述：违章奖励表
    /// </summary>
    public class LllegalRewardController : MvcControllerBase
    {
        private LllegalRegisterBLL lllegalregisterbll = new LllegalRegisterBLL();
        private LllegalRewardBLL lllegalrewardbll = new LllegalRewardBLL();
        private LllegalRewardSettingBLL lllegalrewardsettingbll = new LllegalRewardSettingBLL();
        private HTBaseInfoBLL htbaseinfobll = new HTBaseInfoBLL(); //隐患基本信息
        #region 视图功能
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 违章奖励设置页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SettingForm()
        {
            return View();
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = lllegalrewardbll.GetEntity(keyValue);
            return ToJsonResult(data);
        }

        /// <summary>
        /// 获取实体 
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns>返回对象Json</returns>
        [HttpGet]
        public ActionResult GetFormJsonByOrg()
        {
            Operator curUser = OperatorProvider.Provider.Current();
            var data = lllegalrewardsettingbll.GetList("").Where(p => p.ORGANIZEID == curUser.OrganizeId).FirstOrDefault();
            return ToJsonResult(data);
        }

        #endregion


        #region 获取违章奖励列表数据
        /// <summary>
        /// 获取违章奖励列表数据
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {

            var watch = CommonHelper.TimerStart();
            Operator opertator = new OperatorProvider().Current();

            string strwhere = string.Empty;

            var queryParam = queryJson.ToJObject();

            //部门
            if (null != queryParam["deptcode"] && !string.IsNullOrEmpty(queryParam["deptcode"].ToString()))
            {
                strwhere += string.Format(@" and a.createuserdeptcode like '{0}%'", queryParam["deptcode"].ToString());
            }
            //创建时间开始时间
            if (!string.IsNullOrEmpty(queryParam["startdate"].ToString()))
            {
                strwhere += string.Format(@" and a.createdate >= to_date('{0}','yyyy-mm-dd hh24:mi:ss')", queryParam["startdate"].ToString());
            }
            //创建时间结束时间
            if (!string.IsNullOrEmpty(queryParam["enddate"].ToString()))
            {
                strwhere += string.Format(@" and a.createdate < to_date('{0}','yyyy-mm-dd hh24:mi:ss')", Convert.ToDateTime(queryParam["enddate"].ToString()).AddDays(1).ToString("yyyy-MM-dd"));
            }
          
            //用户
            if (null != queryParam["userid"] && !string.IsNullOrEmpty(queryParam["userid"].ToString()))
            {
                string userids = "'" + queryParam["userid"].ToString().Replace(",", "','") + "'";
                strwhere += string.Format(@" and a.createuserid in ({0})", userids);
            }

            pagination.p_fields = "createusername,createuserid,deptname,deptid,ybwz,jyzwz,yzwz,total";


            //状态
            if (!string.IsNullOrEmpty(queryParam["status"].ToString()))
            {
                if (queryParam["status"].ToString() == "未确认")
                {
                    strwhere += string.Format(@" and b.status  is null");

                    pagination.p_tablename = string.Format(@" ( 
                                                        select a.createusername,a.createuserid ,a.deptname,a.belongdepartid deptid ,nvl(a.ybwz,0) ybwz,nvl(a.jyzwz,0) jyzwz,nvl(a.yzwz,0) yzwz, 
                                                        (nvl(a.ybwz,0)* b.ybpoint + nvl(a.jyzwz,0)*b.jyzpoint + nvl(a.yzwz,0)* b.yzpoint) total,sortcode,encode from (
                                                            select count(a.id) pnum,a.lllegallevelname,a.createusername,a.createuserid, c.fullname deptname, a.belongdepartid,d.sortcode,c.encode from  v_lllegalbaseinfo a 
                                                            left join  bis_lllegalreward b on a.id = b.lllegalid
                                                            left join base_department c on a.createuserdeptcode = c.encode
                                                            left join base_user d on a.createuserid =d.userid
                                                            where  flowstate ='流程结束' {0} group by a.lllegallevelname,a.createusername,a.createuserid,c.fullname,a.belongdepartid ,d.sortcode,c.encode
                                                         ) pivot (sum(pnum) for  lllegallevelname in ('一般违章' as ybwz,'较严重违章' as jyzwz,'严重违章' as yzwz)) a
                                                         left join bis_lllegalrewardsetting b  on a.belongdepartid = b.organizeid
                                                      ) a", strwhere);
                }
                else
                {
                    strwhere += string.Format(@" and b.status ='已确认'");

                    pagination.p_tablename = string.Format(@" ( 
                                                            select a.createusername,a.createuserid ,a.deptname,a.belongdepartid deptid,nvl(a.ybwz,0) ybwz,nvl(a.jyzwz,0) jyzwz,nvl(a.yzwz,0) yzwz,b.total, sortcode,encode from (
                                                                  select count(a.id) pnum, a.lllegallevelname,a.createusername,a.createuserid, c.fullname deptname, a.belongdepartid,d.sortcode,c.encode from  v_lllegalbaseinfo a 
                                                                  left join  bis_lllegalreward b on a.id = b.lllegalid
                                                                  left join base_department c on a.createuserdeptcode = c.encode
                                                                  left join base_user d on a.createuserid =d.userid
                                                                  where  flowstate ='流程结束'  {0}  group by a.lllegallevelname,a.createusername,a.createuserid,c.fullname,a.belongdepartid ,d.sortcode,c.encode
                                                               ) pivot (sum(pnum) for  lllegallevelname in ('一般违章' as ybwz,'较严重违章' as jyzwz,'严重违章' as yzwz)) a
                                                            left join (
                                                                  select  a.createuserid, sum(b.lllegalpoint) total from  v_lllegalbaseinfo a 
                                                                  left join  bis_lllegalreward b on a.id = b.lllegalid
                                                                  where  a.flowstate ='流程结束'  {0} group by a.createuserid
                                                            ) b  on a.createuserid = b.createuserid
                                                      ) a", strwhere);
                }
            }


            //查询违章内容
            if (null != queryParam["querymode"] && !string.IsNullOrEmpty(queryParam["querymode"].ToString()))
            {
                pagination.p_kid = "id";

                pagination.p_fields = "flowstate,createusername,createuserid,deptname,createdate,lllegallevelname,lllegaltypename,lllegalperson,lllegaldescribe,lllegaltime,lllegaladdress,lllegalpoint,addtype";

                pagination.p_tablename = string.Format(@" ( 
                                                         select a.id,a.flowstate,a.addtype, a.createusername, a.createuserid ,c.fullname deptname,a.createdate,a.lllegallevelname,a.lllegaltypename,a.lllegalperson,a.lllegaldescribe,
                                                         a.lllegaltime,a.lllegaladdress,(case when a.lllegallevelname ='一般违章'  then d.ybpoint when a.lllegallevelname ='较严重违章' then d.jyzpoint when a.lllegallevelname ='严重违章'  then d.yzpoint else 0 end) lllegalpoint  from  v_lllegalbaseinfo a 
                                                         left join  bis_lllegalreward b on a.id = b.lllegalid
                                                         left join base_department c on a.createuserdeptcode = c.encode             
                                                         left join bis_lllegalrewardsetting d  on a.belongdepartid = d.organizeid  where  flowstate ='流程结束' {0} 
                                                      ) a", strwhere);
            }
            pagination.conditionJson = "1=1";
            var data = lllegalregisterbll.GetGeneralQuery(pagination);
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch)
            };
            return Content(JsonData.ToJson());

        }
        #endregion

        #region 提交数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult RemoveForm(string keyValue)
        {
            lllegalrewardbll.RemoveForm(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [ValidateInput(false)]
        public ActionResult SaveForm(string keyValue)
        {
            try
            {
                string lllegaldata = Request.Form["lllegaldata"].ToString();

                string rnumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                if (!string.IsNullOrEmpty(lllegaldata))
                {
                    JArray jarray = (JArray)JsonConvert.DeserializeObject(lllegaldata);
                    foreach (JObject rhInfo in jarray)
                    {
                        string id = rhInfo["id"].ToString(); //违章id 
                        string lllegalpoint = rhInfo["lllegalpoint"].ToString(); //违章积分 
                        LllegalRewardEntity dentity = new LllegalRewardEntity();
                        dentity.LLLEGALID = id;
                        dentity.LLLEGALPOINT = lllegalpoint;
                        dentity.REWARDNUMBER = rnumber;
                        dentity.LLLEGALINFO = JsonConvert.SerializeObject(rhInfo);
                        dentity.STATUS = "已确认";
                        lllegalrewardbll.SaveForm(keyValue, dentity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
            return Success("操作成功。");
        }


        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveSettingForm(string keyValue, LllegalRewardSettingEntity entity)
        {
            Operator curUser = OperatorProvider.Provider.Current();
            var data = lllegalrewardsettingbll.GetList("");
            if (data.Count() > 0)
            {
                var curEntity = data.FirstOrDefault();
                curEntity.YBPOINT = entity.YBPOINT;
                curEntity.JYZPOINT = entity.JYZPOINT;
                curEntity.YZPOINT = entity.YZPOINT;
                lllegalrewardsettingbll.SaveForm(curEntity.ID, curEntity);
            }
            else
            {
                entity.ORGANIZEID = curUser.OrganizeId;
                entity.ORGANIZENAME = curUser.OrganizeName;
                lllegalrewardsettingbll.SaveForm(keyValue, entity);
            }
            return Success("操作成功。");
        }

        #endregion
    }
}