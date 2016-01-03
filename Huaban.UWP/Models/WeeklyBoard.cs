using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	public class WeeklyBoard
	{
		public int mDisplyOrder;
		public List<WeeklyHotBoardImg> mHotImg;
		public String[] mSelectedPins;
		public int mShowInEmail;
		public int mShowInWeb;
		public String mTitle;
		public List<WeeklyTopicBoardImg> mTopicImg;
		public String mUserId;
		public int mWeeklyId;
		
	}
}
