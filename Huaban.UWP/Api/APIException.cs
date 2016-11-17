using System;

namespace Huaban.UWP.Api
{
	public class APIException : Exception
	{
		public static int HUABAN = 0x2;
		public static int NETWORK = 0x1;
		private int Code;
		private String Msg;
		public APIException(int code, String msg) : base(code + ":" + msg)
		{
			Code = code;
			Msg = msg;
		}
		public APIException(int code, String msg, Exception e) : base(code + ":" + msg, e)
		{
			Code = code;
			Msg = msg;
		}

		public int getCode()
		{
			return Code;
		}

		public String getMsg()
		{
			return Msg;
		}

		public static APIException network()
		{
			return new APIException(0x1, "\u7f51\u7edc\u5f02\u5e38");
		}

		public static APIException network(String message)
		{
			return new APIException(0x1, message);
		}

		public static APIException network(Exception e)
		{
			return new APIException(0x1, "\u7f51\u7edc\u5f02\u5e38", e);
		}

		public static APIException huaban(int code, String msg)
		{
			code = code + 0x2710;
			return new APIException(code, msg);
		}

	}
}
