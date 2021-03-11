using ERCHTMS.Busines.BaseManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Data;
using ERCHTMS.Entity.BaseManage;
using BSFramework.Util;
using BSFramework.Util.Extension;
using ERCHTMS.Busines.SystemManage;
using System.Web;
using ERCHTMS.AppSerivce.Models;

namespace ERCHTMS.AppSerivce.Controllers
{
    /// <summary>
    /// 区域
    /// </summary>
    public class DistrictController : ApiController
    {
        private DistrictBLL districtBll = new DistrictBLL();

        [HttpPost]
        public List<DistrictEntity> GetDistrict(ParameterModel<DistrictModel> parameterModel)
        {
            //var data = districtBll.GetOrgList(parameterModel.Data).ToList();
            //return new ListResult<DistrictEntity> { Data = data, Success = true, Total = data.Count };

            var data = districtBll.GetDistricts(parameterModel.Data.CompanyId, parameterModel.Data.DistrictId);
            return data;
        }

    }
}