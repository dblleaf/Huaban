using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Huaban.UWP.Services
{
    using Models;

    public class UserService : ServiceBase
    {
        private const string API_SIGNUP = "http://api.huaban.com/signup/";

        public UserService(IClient client)
            : base(client) { }

        public async Task<User> GetSelf()
        {
            return await Get(Constants.API_ME, o => SerializeExtension.JsonDeserlialize<User>(o));
        }
        public async Task<User> GetUser(string userID)
        {
            string uri = $"{Constants.API_USER}{userID}";
            string json = await Get(uri);
            var obj = JObject.Parse(json);
            var user = User.Parse(obj);
            return user;
        }
        //某人的画板
        public async Task<List<Board>> GetBoards(string userID, long max)
        {
            string maxBoardID = "&max=" + max.ToString();
            if (max <= 0)
                maxBoardID = "";
            string uri = $"{Constants.API_USER}{userID}/boards/?limit=20{maxBoardID}";
            string json = await Get(uri);

            var obj = JObject.Parse(json);
            var list = Board.ParseList(obj["boards"] as JArray);
            return list;
        }

        //关注的采集
        public async Task<List<Pin>> GetFollowing(long max)
        {
            string maxPin = "";
            if (max > 0)
                maxPin = "&max=" + max;
            string uri = $"{Constants.API_Follow}?limit=20{maxPin}";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = Pin.ParseList(obj["pins"] as JArray);
            return list;
        }
        //某人的采集
        public async Task<List<Pin>> GetPins(string userID, long max)
        {
            string maxPin = "";
            if (max > 0)
                maxPin = "&max=" + max;

            string uri = $"{Constants.API_USER}{userID}/pins/?limit=20{maxPin}";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = Pin.ParseList(obj["pins"] as JArray);
            return list;
        }
        //某人喜欢的采集
        public async Task<List<Pin>> GetLikePins(string userID, long max)
        {
            string maxPin = "";
            if (max > 0)
                maxPin = "&max=" + max;

            string uri = $"{Constants.API_USER}{userID}/likes/?limit=20{maxPin}";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = Pin.ParseList(obj["pins"] as JArray);
            return list;
        }
        //某人关注的画板
        public async Task<List<Board>> GetFollowingBoardList(string userID, int page = 0)
        {
            if (page <= 0)
                page = 1;
            string uri = $"{Constants.API_MAIN}{userID}/following/boards/?page={page}&per_page=20&wfl=1";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = Board.ParseList(obj["boards"] as JArray);
            return list;
        }

        //某人的粉丝
        public async Task<List<User>> GetFollowerList(string userID, long max, int limit = 20)
        {
            string maxUser = "";
            if (max > 0)
                maxUser = "&max=" + max;

            string uri = $"{Constants.API_USER}{userID}/followers/?limit={limit}{maxUser}&wfl=1";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = User.ParseList(obj["users"] as JArray);
            return list;
        }

        //某人关注的人
        public async Task<List<User>> GetFollowingUserList(string userID, long max, int limit = 20)
        {
            string maxUser = "";
            if (max > 0)
                maxUser = "&max=" + max;

            string uri = $"{Constants.API_USER}{userID}/following/?limit={limit}{maxUser}&wfl=1";
            string json = await Get(uri);
            JObject obj = JObject.Parse(json);
            var list = User.ParseList(obj["users"] as JArray);
            return list;
        }

        public async Task<string> follow(string userID, bool follow)
        {
            string action = follow ? "follow" : "unfollow";
            string uri = $"{Constants.API_USER}{userID}/{action}/";
            return await Post(uri);
        }
    }
}
