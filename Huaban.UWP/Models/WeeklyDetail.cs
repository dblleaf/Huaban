using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	class WeeklyDetail
	{
		protected static String HOT_BOARDS = "hot_boards";
		protected static String HOT_PINS = "hot_pins";
		protected static String TOPIC_BOARDS = "topic_boards";
		public List<WeeklyBoard> mHotBoards;
		public List<Pin> mHotPins;
		public List<WeeklyBoard> mTopicBoards;
		
	}
}
