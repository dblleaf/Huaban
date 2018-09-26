using System.Collections.Generic;

namespace iHuaban.App.Models
{
    public class UserCollection: IModelCollection<User>
    {
        public List<User> Users { set; get; }
        public int Count => Users.Count;
        public IEnumerable<User> Data => Users;
    }
}
