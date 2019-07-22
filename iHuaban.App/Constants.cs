using iHuaban.App.Models;

namespace iHuaban.App
{
    public class Constants
    {
        #region Api
        public const string ApiBase = "http://api.huaban.com/";
        public const string ApiBoardsName = "boards";
        public const string ApiCategoriesName = "categories";
        public const string ApiFavoriteName = "favorite";
        public const string ApiPinsName = "pins";
        public const string ApiBoards = "http://api.huaban.com/boards/";
        public const string ApiCategories = "http://api.huaban.com/categories/";
        public const string ApiUsers = "http://api.huaban.com/users/";
        public const string ApiFavorite = "http://api.huaban.com/favorite/";

        public const string ApiFavoritePins = "http://api.huaban.com/favorite/";
        public const string ApiFavoriteBoards = "http://api.huaban.com/boards/favorite/";
        public const string ApiFavoriteUsers = "http://api.huaban.com/users/favorite/";

        public const string Apifeeds = "http://api.huaban.com/feeds/";
        public const string ApiFollow = "http://api.huaban.com/following/";
        public const string ApiFriends = "http://api.huaban.com/friends/";

        public const string ApiSearchPins = "http://api.huaban.com/search/";
        public const string ApiSearchBoards = "http://api.huaban.com/search/boards/";
        public const string ApiSearchUsers = "http://api.huaban.com/search/people/";

        public const string UrlLogin = "https://huaban.com/oauth";

        public static readonly Category CategoryAll = new Category { name = "最新", nav_link = "/all/" };
        public static readonly Category CategoryHot = new Category { name = "最热", nav_link = "/popular/" };

        public const string DefaultFavorite = "all";
        #endregion

        #region Template

        public const string TemplateSearch = "TemplateSearch";
        public const string TemplateMine = "TemplateMine";
        public const string TemplateHome = "TemplateHome";
        public const string TemplateFind = "TemplateFind";
        public const string TemplateCategory = "TemplateCategory";
        public const string TemplateCurrentUser = "TemplateCurrentUser";
        public const string TemplateLogin = "TemplateLogin";
        #endregion

        #region Text
        public const string TextHome = "首页";
        public const string TextCategory = "分类";
        public const string TextFind = "发现";
        public const string TextMine = "我的";
        public const string TextSetting = "设置";
        public const string TextAbout = "关于";
        public const string TextRefresh = "刷新";
        #endregion

        #region Icon
        public const string IconHome = "\uE80F";
        public const string IconCategory = "\uE8FD";
        public const string IconFind = "\uE721";
        public const string IconMine = "\uE77B";
        public const string IconSetting = "\uE713";

        public const string IconAbout = "\uE9CE";
        public const string IconRefesh = "\uE72C";
        #endregion

        #region Auth

        public const string ClientId = "";
        public const string ClientSecret = "";
        public const string ClientInfo = ClientId + ":" + ClientSecret;
        public const string OAuthCallback = "ms-appx-web:///Assets/authcallback.html";
        public const string MD = "com.huaban.android";

        public const string Authorization = "Authorization";
        public const string ClientIdKey = "X-Client-ID";
        public const string ClientSecretKey = "ClientSecret";

        #endregion
    }
}
