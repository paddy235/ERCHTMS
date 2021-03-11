using Nancy;

namespace Web.Modules
{
    public class ErrorModule : NancyModule
    {
        public ErrorModule()
            : base("Error")
        {
            //404
            Get["/NotFound"] = r =>
            {
                return View["NotFound"];
            };

            //500
            Get["/ISE"] = r =>
            {
                var model = "我是 Razor 引擎";
                return View["ISE", model];
            };
        }
    }
}
