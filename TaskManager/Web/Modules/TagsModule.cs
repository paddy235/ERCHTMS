using Nancy;
using Utility.Mef;
using Utility.Tag;

namespace Web.Modules
{
    public class TagsModule : BaseModule
    {
        public TagsModule()
            : base("Tags")
        {

            #region "取数接口API"

            //立即运行一次任务
            Get["/UpdateTagHeat"] = r =>
            {
                string TagGUID = Request.Query["TagGUID"].ToString();
                ITagService tagService = MefConfig.TryResolve<ITagService>();
                return Response.AsText(tagService.UpdateHeat(TagGUID).ToString());
            };


            #endregion
        }
    }
}
