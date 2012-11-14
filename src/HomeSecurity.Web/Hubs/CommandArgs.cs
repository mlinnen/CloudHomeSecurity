using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
	public class CommandEventArgs
	{
		public CommandEventArgs()
		{

		}

		public CommandEventArgs(string houseCode, string deviceCode, string locationCode, string command, string commandValue)
		{
			HouseCode = houseCode;
			DeviceCode = deviceCode;
			LocationCode = locationCode;
			Command = command;
			CommandValue = commandValue;
		}

		public CommandEventArgs(string topic, string msg)
		{
			string[] topicArgs = topic.Split('/');
			if (topicArgs.Length > 0)
			{
				HouseCode = topicArgs[0];
			}
			if (topicArgs.Length > 1)
			{
				DeviceCode = topicArgs[1];
			}
			if (topicArgs.Length > 2)
			{
				LocationCode = topicArgs[2];
			}
			if (topicArgs.Length > 3)
			{
				Command = topicArgs[3];
			}
			if (topicArgs.Length > 4)
			{
				CommandValue = topicArgs[4];
			}

		}
		public string HouseCode { get; set; }
		public string DeviceCode { get; set; }
		public string LocationCode { get; set; }
		public string Command { get; set; }
		public string CommandValue { get; set; }

		public string GetTopic()
		{
			return String.Format("{0}/{1}/{2}/{3}", HouseCode, DeviceCode, LocationCode, Command);
		}

		public string GetMessage()
		{
			return CommandValue;
		}
	}
}