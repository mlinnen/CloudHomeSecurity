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

        public static CommandEventArgs BuildCommandArgs(string topic, string msg)
        {
            string[] topicArgs = topic.Split('/');
            if (topicArgs.Length < 5)
                return null;

            CommandEventArgs args = new CommandEventArgs();
            args.HouseCode = topicArgs[1];
            args.DeviceCode = topicArgs[2];
            args.LocationCode = topicArgs[3];
            args.Command = topicArgs[4];
            args.CommandValue = msg;

            return args;
        }
	}
}