using BSFramework.Util.WebControl;
using ERCHTMS.Busines.AccidentEvent;
using ERCHTMS.Code;
using ERCHTMS.Entity.AccidentEvent;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ERCHTMS.AppSerivce.Controllers
{
    public class AccidentEventController : BaseApiController
    {
        public BulletinBLL bulletinbll = new BulletinBLL();
        public Bulletin_dealBLL bulletin_dealbll = new Bulletin_dealBLL();
        public WSSJBGBLL wssjbgbll = new WSSJBGBLL();

        #region 事故事件快报
        /// <summary>
        /// 事故事件快报
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetBulletinList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string sgName = dy.SgName ?? "";//事故名称
            string sgType = dy.SgType ?? "";//事故类型
            string happenTimeStart = dy.HappenTimeStart ?? "";//发生时间
            string happenTimeEnd = dy.HappenTimeEnd ?? "";//发生时间
            string sgkbUserId = dy.SgkbUserId ?? "";//报告人
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            string roleNames = curUser.RoleName;

            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page ?? "1");
            pagination.rows = int.Parse(dy.rows ?? "1");
            pagination.p_kid = "ID";
            pagination.p_fields = "SgName,HappenTime,AreaName,AreaId,Files";
            pagination.p_tablename = "V_AEM_BULLETIN t";
            pagination.sidx = "HappenTime";
            pagination.sord = "desc";
            pagination.conditionJson = "CREATEUSERORGCODE='" + curUser.OrganizeCode + "'";

            //查询条件
            if (happenTimeStart.Length > 0)
                pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happenTimeStart);
            if (happenTimeEnd.Length > 0)
                pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happenTimeEnd);
            if (sgName.Length > 0)
                pagination.conditionJson += string.Format(" and SGNAME  like '%{0}%'", sgName);
            if (sgType.Length > 0)
                pagination.conditionJson += string.Format(" and sgtype = '{0}'", sgType);
            if (sgkbUserId.Length > 0)
                pagination.conditionJson += string.Format(" and sgkbUserId = '{0}'", sgkbUserId);
            //获取数据
            var data = bulletinbll.GetPageList(pagination, "");
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            return new { code = 0, info = "获取数据成功", data = JsonData };

        }


        #endregion

        #region 新增快报
        /// <summary>
        /// 新增快报
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddBulletin([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var entity = new BulletinEntity
                {
                    SGNAME = dy.SgName,
                    SGTYPE = dy.SgType,
                    SGTYPENAME = dy.SgTypeName,
                    HAPPENTIME = dy.HappenTime,
                    SGLEVEL = dy.SgLevel,
                    AREAID = dy.AreaId,
                    AREANAME = dy.AreaName,
                    SGLEVELNAME = dy.SgLevelName,
                    RSSHSGTYPE = dy.RsshsgType,
                    RSSHSGTYPENAME = dy.RsshsgTypeName,
                    ISTZSBSG = Convert.ToInt32(dy.IstzsbSg),
                    EQUIPMENTNAME = dy.EquipmentName,
                    EQUIPMENTID = dy.EquipmentId,
                    JYJG = dy.Jyjg,
                    SWNUM = Convert.ToInt32(dy.SwNum),
                    ZSNUM = Convert.ToInt32(dy.ZsNum),
                    SZNUM = Convert.ToInt32(dy.SzNum),
                    QSNUM = Convert.ToInt32(dy.QsNum),
                    SHQKSHJE = dy.Shqkshje,
                    TDQK = dy.Tdqk,
                    CBYY = dy.Cbyy,
                    HFQK = dy.Hfqk,
                    FILES = dy.Files,
                    DEPARTMENTID = dy.DepartmentId,
                    DEPARTMENTNAME = dy.DepartmentName,
                    SGKBUSERID = dy.SgkbUserId,
                    SGKBUSERNAME = dy.SgkbUserName,
                    TBTIME = dy.Tbtime,
                    MOBILE = dy.Mobile
                };
                bulletinbll.SaveForm("", entity);
            }

            catch (Exception)
            {

                return new { code = -0, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

        #region 未遂事件报告
        /// <summary>
        /// 未遂事件报告
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object GetWssjBgList([FromBody]JObject json)
        {
            string res = json.Value<string>("json");
            dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
            string userid = dy.userid;
            string wssjName = dy.WssjName ?? "";//事故名称
            string wssjType = dy.WssjType ?? "";//事故类型
            string happenTimeStart = dy.HappenTimeStart ?? "";//发生时间
            string happenTimeEnd = dy.HappenTimeEnd ?? "";//发生时间
            string wssjbgUserId = dy.WssjbgUserId ?? "";//报告人
            //获取用户基本信息
            OperatorProvider.AppUserId = userid;  //设置当前用户
            Operator curUser = OperatorProvider.Provider.Current();
            if (null == curUser)
            {
                return new { code = -1, count = 0, info = "请求失败,请登录!" };
            }
            //分页获取数据
            Pagination pagination = new Pagination();
            pagination.page = int.Parse(dy.page ?? "1");
            pagination.rows = int.Parse(dy.rows ?? "1");
            pagination.p_kid = "ID";
            pagination.p_fields = "WssjName,HappenTime,AreaName,AreaId,WSSJTPSP";
            pagination.p_tablename = "V_AEM_WSSJBG t";
            pagination.sidx = "HappenTime";
            pagination.sord = "desc";
            pagination.conditionJson = "CREATEUSERORGCODE='" + curUser.OrganizeCode + "'";


            //查询条件
            if (happenTimeStart.Length > 0)
                pagination.conditionJson += string.Format(" and happentime >= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happenTimeStart);
            if (happenTimeEnd.Length > 0)
                pagination.conditionJson += string.Format(" and happentime <= (select  to_date('{0}', 'yyyy-MM-dd HH24:mi:ss') from dual)", happenTimeEnd);
            if (wssjType.Length > 0)
                pagination.conditionJson += string.Format(" and WSSJTYPE = '{0}'", wssjType);
            if (wssjName.Length > 0)
                pagination.conditionJson += string.Format(" and WSSJNAME  like '%{0}%'", wssjName);

            if (wssjbgUserId.Length > 0)
                pagination.conditionJson += string.Format(" and WSSJBGUSERID = '{0}'", wssjbgUserId);

            
            //获取数据
            var data = wssjbgbll.GetPageList(pagination, "");
            var JsonData = new
            {
                rows = data,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records,
            };
            return new { code = 0, info = "获取数据成功", data = JsonData };

        }


        #endregion

        #region 新增未遂事件报告
        /// <summary>
        /// 新增快报
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public object AddWssjBg([FromBody]JObject json)
        {
            try
            {
                string res = json.Value<string>("json");
                dynamic dy = JsonConvert.DeserializeObject<ExpandoObject>(res);
                string userid = dy.userid;
                //获取用户基本信息
                OperatorProvider.AppUserId = userid;  //设置当前用户
                Operator curUser = OperatorProvider.Provider.Current();
                if (null == curUser)
                {
                    return new { code = -1, count = 0, info = "请求失败,请登录!" };
                }
                var entity = new WSSJBGEntity
                {
                    WSSJNAME = dy.WssjName,
                    WSSJTYPE = dy.WssjType,
                    WSSJTYPENAME = dy.WssjTypeName,
                    HAPPENTIME = dy.HappenTime,
                    AREAID = dy.AreaId,
                    AREANAME = dy.AreaName,
                    JYJG = dy.Jyjg,
                    HGHYX = dy.Hghyx,
                    YYCBPD = dy.Yycbpd,
                    CQDCS = dy.Cqdcs,
                    WSSJTPSP = dy.Wssjtpsp,
                    WSSJBGUSERID = dy.WssjbgUserId,
                    WSSJBGUSERNAME = dy.WssjbgUserName,
                    TBTIME = dy.Tbtime,
                    BGDEPARTID = dy.BgDepartId,
                    BGDEPARTNAME = dy.BgDepartName,
                };
                wssjbgbll.SaveForm("", entity);
            }
            catch (Exception)
            {

                return new { code = -0, count = 0, info = "保存失败" };
            }
            return new { code = 0, count = 0, info = "保存成功" };
        }
        #endregion

    }
}
