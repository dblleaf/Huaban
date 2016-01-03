using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huaban.UWP.Models
{
	class Message
	{
		private static String ACTIVITIES = "activities";
		private static String MENTIONS = "mentions";
		private static String MESSAGE_TYPE = "message_type";
		public List<Feed> mFeeds;
		public Message.MessageType mMessageType;
		public List<Pin> mPins;
		public User mUser;
		
		public enum MessageType
		{

		}
	}
}
