using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class UserCollection: ModelCollection<User>
    {
        public List<User> Users { set; get; }
        public override IEnumerable<User> Data => Users;
    }

    public class FavoriteUserCollection : ModelCollection<PUser>
    {
        public List<PUser> PUsers { set; get; }
        public override IEnumerable<PUser> Data => PUsers;
    }
}
