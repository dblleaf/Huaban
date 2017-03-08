using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Huaban.UWP.ViewModels
{
    using Base;
    using Commands;
    using Models;
    public class UserListViewModel : HBViewModel
    {
        public UserListViewModel(Context context, Func<uint, int, Task<IEnumerable<User>>> _func)
            : base(context)
        {
            UserList = new IncrementalLoadingList<User>(_func);
        }

        #region Properties

        private IncrementalLoadingList<User> _UserList;
        public IncrementalLoadingList<User> UserList
        {
            get { return _UserList; }
            set
            { SetValue(ref _UserList, value); }
        }

        public int Count
        {
            get { return UserList.Count; }
        }

        #endregion

        #region Commands

        #endregion

        #region Methods

        public void Clear()
        {
            UserList.Clear();
        }

        public async Task ClearAndReload()
        {
            await UserList.ClearAndReload();
        }

        public long GetMaxSeq()
        {
            long max = 0;
            if (Count > 0)
                max = Convert.ToInt64(UserList[Count - 1].seq);
            return max;
        }

        public long GetMaxID()
        {
            long max = 0;
            if (Count > 0)
                max = Convert.ToInt64(UserList[Count - 1].user_id);
            return max;
        }

        public override void Dispose()
        {
            Clear();
            base.Dispose();
        }
        #endregion
    }
}
