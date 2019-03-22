namespace iHuaban.App.Models
{
    public class AccessTokenParameter
    {
        public string grant_type => "password";
        public string username { get; set; }
        public string password { get; set; }
    }
}
