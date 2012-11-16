﻿using MqttLib;
using SignalR;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
	public class MasterControlPanel
	{
		private AlarmState _currentState = AlarmState.Off;
		private int _secretCode = 01;
		private readonly IMqtt _client;
		private Timer _delayAlarm;
		private int _delayInMilliseconds = 10000;
		private CommandEventArgs _currentEventArgs;
        private List<SecuritySensor> _sensors = new List<SecuritySensor>();
        private bool _alarmSounding;

		public MasterControlPanel(IMqtt client)
		{
			_client = client;
			_delayAlarm = new Timer(_delayInMilliseconds);
			_delayAlarm.Enabled = false;
			_delayAlarm.Elapsed += _delayAlarm_Elapsed;

            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "front", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "side", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "back", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "firstfloor", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "masterbedroom", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom1", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom2", Sensor = "window", SensorValue = "closed" });

        }

        public bool ProcessCommand(CommandEventArgs args)
        {
            if (!(args == null ||
                string.IsNullOrEmpty(args.HouseCode) ||
                string.IsNullOrEmpty(args.DeviceCode) ||
                string.IsNullOrEmpty(args.Command) ||
                string.IsNullOrEmpty(args.CommandValue)))
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeSecurityHub>();

                switch (args.DeviceCode)
                {
                    case "externaldoor":
                        if (args.Command.Equals("code"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            if (UnLockDoor(args))
                                DisarmAlarm(args);
                        }
                        if (args.Command.Equals("doorbell"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);
                        }
                        if (args.Command.Equals("door"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("window"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);
                            
                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("motion"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);
                            
                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("lock"))
                        {
                            // TODO Notify the webpage
                            context.Clients.updateCommand(args);
                        }
                        break;
                    case "alarmpanel":
                        if (args.Command.Equals("alarmstate"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            SetAlarmState(args);
                        }
                        if (args.Command.Equals("emergency"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            SoundBurglarAlarm(args);
                        }
                        if (args.Command.Equals("code"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            DisarmAlarm(args);
                        }
                        if (args.Command.Equals("door"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("window"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("motion"))
                        {
                            // TODO Notify the web page
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        break;
                }
            }
            else
                return false;
            return true;
        }

		private void SetAlarmState(CommandEventArgs args)
		{
            AlarmState newState = ParseAlarmState(args.CommandValue);
			if (newState == _currentState)
				return;

			if (_currentState == AlarmState.Off)
			{
                // You cant set the alarm if any of the windows, doors or motion detectors are currently open
                int openedSensorCount = _sensors.Where(s => s.SensorValue == "opened").Count();
                if (openedSensorCount > 0)
                {
                    string topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "firstfloor", "alarmstatevalid");
                    _client.Publish(topic, new MqttPayload("false"), QoS.BestEfforts, false);

                    topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "masterbedroom", "alarmstatevalid");
                    _client.Publish(topic, new MqttPayload("false"), QoS.BestEfforts, false);

                    topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "bedroom1", "alarmstatevalid");
                    _client.Publish(topic, new MqttPayload("false"), QoS.BestEfforts, false);

                    topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "bedroom2", "alarmstatevalid");
                    _client.Publish(topic, new MqttPayload("false"), QoS.BestEfforts, false);
                }
                else
                {
                    _currentState = newState;
                    SendAlarmStateChange(args.HouseCode, _currentState);
                    if (_currentState == AlarmState.Sleep)
                        LockAllDoors();
                }
			}
		}

        private void SendAlarmStateChange(string houseCode, AlarmState state)
        {
            string topic = string.Format("/{0}/{1}/{2}/{3}", houseCode, "alarmpanel", "firstfloor", "setalarmstate");
            _client.Publish(topic, new MqttPayload(GetAlarmState(state)), QoS.BestEfforts, false);
            topic = string.Format("/{0}/{1}/{2}/{3}", houseCode, "alarmpanel", "masterbedroom", "setalarmstate");
            _client.Publish(topic, new MqttPayload(GetAlarmState(state)), QoS.BestEfforts, false);
            topic = string.Format("/{0}/{1}/{2}/{3}", houseCode, "alarmpanel", "bedroom1", "setalarmstate");
            _client.Publish(topic, new MqttPayload(GetAlarmState(state)), QoS.BestEfforts, false);
            topic = string.Format("/{0}/{1}/{2}/{3}", houseCode, "alarmpanel", "bedroom2", "setalarmstate");
            _client.Publish(topic, new MqttPayload(GetAlarmState(state)), QoS.BestEfforts, false);

        }

        private bool UnLockDoor(CommandEventArgs args)
        {
            int code = 0;
            bool unlockedDoor = false;

            int.TryParse(args.CommandValue, out code);

            string topic = string.Format("/{0}/{1}/{2}/{3}", args.HouseCode, args.DeviceCode, args.LocationCode, "codevalid");
            if (code == _secretCode)
            {
                _client.Publish(topic, new MqttPayload("true"), QoS.BestEfforts, false);
                topic = string.Format("/{0}/{1}/{2}/{3}", args.HouseCode, args.DeviceCode, args.LocationCode, "setlock");
                _client.Publish(topic, new MqttPayload("unlock"), QoS.BestEfforts, false);
                unlockedDoor = true;
            }
            else
                _client.Publish(topic, new MqttPayload("false"), QoS.BestEfforts, false);

            return unlockedDoor;
        }

        private bool DisarmAlarm(CommandEventArgs args)
		{
            int code = 0;
            bool disarmed=false;

            int.TryParse(args.CommandValue, out code);

			if (code == _secretCode)
			{
				_delayAlarm.Enabled = false;
				_currentState = AlarmState.Off;
                SendAlarmStateChange(args.HouseCode, _currentState);
				SilenceBurglarAlarm();
                disarmed = true;
			}
            return disarmed;
		}

		private void ProcessSensorStateChange(CommandEventArgs args)
		{
            if (args == null || args.CommandValue == null || !(args.CommandValue.Equals("opened") || args.CommandValue.Equals("closed")))
				return;

            // Set the new state of the sensors
            var sensor = _sensors.Where(s => s.HouseCode == args.HouseCode
                && s.DeviceCode == args.DeviceCode
                && s.LocationCode == args.LocationCode && s.Sensor == args.Command).FirstOrDefault();
            if (sensor != null)
            {
                sensor.SensorValue = args.CommandValue;
            }

            if (args.CommandValue.Equals("closed"))
            {
                if (_currentState == AlarmState.Sleep)
			    {
				    SoundBurglarAlarm(args);
			    }

			    if (_currentState == AlarmState.Away && _delayAlarm.Enabled == false)
			    {
				    _currentEventArgs = args;
				    // Start a timer that allows the user to get in the door and disarm the alarm before it fires
				    _delayAlarm.Interval = _delayInMilliseconds;
				    _delayAlarm.Enabled = true;
			    }
            }

		}

        private void LockAllDoors()
        {
            string topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "externaldoor", "front", "setlock");
            _client.Publish(topic, new MqttPayload("lock"), QoS.BestEfforts, false);

            topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "externaldoor", "side", "setlock");
            _client.Publish(topic, new MqttPayload("lock"), QoS.BestEfforts, false);

            topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "externaldoor", "back", "setlock");
            _client.Publish(topic, new MqttPayload("lock"), QoS.BestEfforts, false);
        }

		void _delayAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			_delayAlarm.Enabled = false;
			SoundBurglarAlarm(_currentEventArgs);

		}

		private void SoundBurglarAlarm(CommandEventArgs args)
		{
            _alarmSounding = true;
			// Use the args to determine where to send the alarm burglar trigger command
			string topic = string.Format("/{0}/{1}/{2}/{3}", args.HouseCode, args.DeviceCode, args.LocationCode, "burglar");
			_client.Publish(topic, new MqttPayload("on"), QoS.BestEfforts, false);
		}

		private void SilenceBurglarAlarm()
		{
            if (_alarmSounding)
            {
                string topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "masterbedroom", "burglar");
                _client.Publish(topic, new MqttPayload("off"), QoS.BestEfforts, false);

                topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "bedroom1", "burglar");
                _client.Publish(topic, new MqttPayload("off"), QoS.BestEfforts, false);

                topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "bedroom2", "burglar");
                _client.Publish(topic, new MqttPayload("off"), QoS.BestEfforts, false);

                topic = string.Format("/{0}/{1}/{2}/{3}", "house1", "alarmpanel", "firstfloor", "burglar");
                _client.Publish(topic, new MqttPayload("off"), QoS.BestEfforts, false);

                _alarmSounding = false;
            }
		}

        private AlarmState ParseAlarmState(string state)
        {
            AlarmState returnState = AlarmState.Unknown;
            if (state.Equals("off"))
            {
                returnState = AlarmState.Off;
            }
            if (state.Equals("away"))
            {
                returnState = AlarmState.Away;
            }
            if (state.Equals("sleep"))
            {
                returnState = AlarmState.Sleep;
            }

            return returnState;
        }
        private string GetAlarmState(AlarmState state)
        {
            string stringState = "unknown";
            switch (state)
            {
                case AlarmState.Off:
                    stringState = "off";
                    break;
                case AlarmState.Away:
                    stringState = "away";
                    break;
                case AlarmState.Sleep:
                    stringState = "sleep";
                    break;
            }
            return stringState;
        }
    }
}