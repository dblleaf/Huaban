using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class UserCollection: ModelCollection<User>
    {
        public List<User> Users { set; get; }
        public override IEnumerable<User> Data => Users;
    }
}
