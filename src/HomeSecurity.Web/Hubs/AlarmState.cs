using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
	public enum AlarmState
	{
		Off = 0,
		Sleep = 1,
		Away = 2,
        Unknown=3,
	}
}