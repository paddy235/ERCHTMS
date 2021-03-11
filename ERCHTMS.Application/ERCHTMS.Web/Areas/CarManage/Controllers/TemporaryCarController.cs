using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using ERCHTMS.Entity.CarManage;
using ERCHTMS.Busines.CarManage;
using BSFramework.Util;
using BSFramework.Util.WebControl;
using System.Web.Mvc;
using BSFramework.Util.Offices;
using ERCHTMS.Busines.BaseManage;
using ERCHTMS.Busines.SystemManage;
using ERCHTMS.Code;
using ERCHTMS.Entity.BaseManage;
using ERCHTMS.Entity.OccupationalHealthManage;
using ERCHTMS.Entity.SystemManage;
using ERCHTMS.Entity.SystemManage.ViewModel;
using System.Data;
using Aspose.Words.Lists;
using System.Collections.Generic;



namespace ERCHTMS.Web.Areas.CarManage.Controllers
{
    /// <summary>
    /// 厂内临时车
    /// </summary>
    public class TemporaryCarController : MvcControllerBase
    {

        private CarinfoBLL carinfobll = new CarinfoBLL();
        private CargpsBLL gpsbll = new CargpsBLL();
        private DepartmentBLL departmentBLL = new DepartmentBLL();

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

      

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryJson">查询参数</param>
        /// <returns>返回列表Json</returns>
        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string queryJson)
        {
            string orgcode = OperatorProvider.Provider.Current().OrganizeCode;
            var watch = CommonHelper.TimerStart();
            pagination.p_kid = "ID";
            pagination.p_fields = "info.Starttime,info.Endtime,info.Remark,info.Currentgname,info.carno,info.dirver,info.phone,info.numberlimit,info.model,info.insperctiondate,info.nextinsperctiondate,NVL(zvi.num,0) as znum,NVL(vi.num,0) as num";
            pagination.p_tablename = @"bis_carinfo info
          left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   group by CARDNO)
						zvi on info.CARNO=zvi.CARDNO
					 left join	(SELECT COUNT(vi.CARDNO) as num, vi.CARDNO FROM  BIS_CARVIOLATION  vi   where ISPROCESS=0 group by CARDNO)
						vi on info.CARNO=vi.CARDNO
            ";
            pagination.conditionJson = " 1=1";
            Operator user = ERCHTMS.Code.OperatorProvider.Provider.Current();
            //if (user.IsSystem)
            //{
            //    pagination.conditionJson = "1=1";
            //}
            //else
            //{
            //    pagination.conditionJson += " and createuserorgcode like '" + user.OrganizeCode + "%'";
            //}

            var data = carinfobll.GetPageList(pagination, queryJson);

            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
                costtime = CommonHelper.TimerEnd(watch),

            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ActionResult GetUserInfo(string userid)
        {

            var userData = new UserBLL().GetEntity(userid);
            if (userData != null)
            {
                var dept = new DepartmentBLL().GetEntity(userData.DepartmentId);
                if (dept != null) { userData.DepartmentId = dept.FullName; }

            }
            return Content(new { data = userData }.ToJson());
        }

        /// <summary>
        /// 获取平台用户表详细信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserDetails()
        {
            List<CarinfoEntity> userlist = new List<CarinfoEntity>();
            string sql = string.Format(@"select 
             u.userid,Account,senddeptid,REALNAME,MOBILE,OrganizeName,DEPTNAME,DUTYNAME,POSTNAME,usertype,GENDER,OrganizeCode,CreateDate,isblack,identifyid,score,IsPresence,realname as username,DEPARTURETIME,u.deptcode,nature,u.DEPARTMENTCODE,organizeid,u.IsTransfer,u.DepartureReason,u.fourpersontype,1 isleave,IsEpiboly
             from 
             v_userinfo u left join (select a.userid,sum(score) as score from base_user a left join bis_userscore b on a.userid=b.userid where year='{0}' group by a.userid) t on u.userid=t.userid
             where Account!='System' and u.EnabledMark=1", DateTime.Now.Year);
            DataTable dt = departmentBLL.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CarinfoEntity entity = new CarinfoEntity();
                    entity.Dirver = dr["username"].ToString();
                    entity.Phone = dr["MOBILE"].ToString();
                    entity.Remark = dr["DEPTNAME"].ToString();//单位
                    entity.DirverId = dr["userid"].ToString();

                    DataTable dt1 = departmentBLL.GetDataTable(string.Format("select fullname from BASE_DEPARTMENT where encode=(select encode from BASE_DEPARTMENT t where instr('{0}',encode)=1 and nature='{1}' and organizeid='{2}') or encode='{0}' order by deptcode", dr["DEPARTMENTCODE"], "部门", dr["organizeid"]));
                    if (dt1.Rows.Count > 0)
                    {
                        string name = "";
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            name += dr1["fullname"].ToString() + "/";
                        }
                        //dr["deptname"] = name.TrimEnd('/');
                        if (name != null)
                        {
                            entity.Deptname = entity.Dirver + "-" + name.TrimEnd('/');
                        }
                        else
                        {
                            entity.Deptname = entity.Dirver;
                        }
                    }

                    userlist.Add(entity);
                }
            }
            return Content(userlist.ToJson());
        }


        #endregion

        #region 提交数据

        /// <summary>
        /// 保存表单（新增、修改）
        /// </summary>
        /// <param name="keyValue">主键值</param>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult SaveForm(string keyValue, CarinfoEntity entity)
        {
            DataItemDetailBLL pdata = new DataItemDetailBLL();
            var pitem = pdata.GetItemValue("Hikappkey");//海康服务器密钥
            var url = pdata.GetItemValue("HikBaseUrl");//海康服务器地址
            carinfobll.SaveForm(keyValue, entity, pitem, url);
            return Success("操作成功。");
        }
        #endregion



    }
}
