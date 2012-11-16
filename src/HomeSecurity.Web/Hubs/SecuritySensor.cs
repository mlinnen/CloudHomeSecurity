using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
    public class SecuritySensor
    {
        public string HouseCode { get; set; }

        public string DeviceCode { get; set; }

        public string LocationCode { get; set; }

        public string Sensor { get; set; }

        public string SensorValue { get; set; }

    }
}