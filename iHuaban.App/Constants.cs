using iHuaban.App.Models;

namespace iHuaban.App
{
    public class Constants
    {
        #region Api
        public const string ApiBase = "http://api.huabanpro.com/";
        public const string ApiBoardsName = "boards";
        public const string ApiCategoriesName = "categories";
        public const string ApiFavoriteName = "favorite";
        public const string ApiPinsName = "pins";
        public const string ApiBoards = "http://api.huabanpro.com/boards/";
        public const string ApiCategories = "http://api.huabanpro.com/categories/";
        public const string ApiFavorite = "http://api.huabanpro.com/favorite/";

        public const string Apifeeds = "http://api.huabanpro.com/feeds/";
        public const string ApiFollow = "http://api.huabanpro.com/following/";
        public const string ApiFriends = "http://api.huabanpro.com/friends/";

        public static readonly Category CategoryAll = new Category { name = "最新", nav_link = "/all/" };
        public static readonly Category CategoryHot = new Category { name = "最热", nav_link = "/popular/" };

        public const string DefaultFavorite = "all";
        #endregion

        #region Template

        public const string TemplateFind = "TemplateFind";
        public const string TemplateCategories = "TemplateCategories";
        public const string TemplateMine = "TemplateMine";
        public const string TemplateHome = "TemplateHome";

        public const string TemplateCategory = "TemplateCategory";
        #endregion
        public const string TextHome = "首页";
        public const string TextCategory = "分类";
        public const string TextFind = "发现";
        public const string TextMine = "我的";
        public const string TextSetting = "设置";
        public const string TextAbout = "关于";
        public const string TextRefresh = "刷新";

        public const string IconHome = "\uE80F";
        public const string IconCategory = "\uE8FD";
        public const string IconFind = "\uE721";
        public const string IconMine = "\uE77B";
        public const string IconSetting = "\uE713";

        public const string IconAbout = "\uE9CE";
        public const string IconRefesh = "\uE72C";
    }
}
