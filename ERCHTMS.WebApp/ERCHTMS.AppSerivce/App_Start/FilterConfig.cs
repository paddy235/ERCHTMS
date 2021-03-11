using System.Web.Mvc;

namespace ERCHTMS.AppSerivce
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new WebAPIHandlerErrorAttribute());
        }
    }
}