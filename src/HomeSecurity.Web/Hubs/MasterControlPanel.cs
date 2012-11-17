using MqttLib;
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
        private Timer _turnOffDoorbellIndicatorTimer;
        private Timer _giveMeTimeToEnterTimer;
        private Timer _giveMeTimeToExitTimer;
		private int _giveMeTimeToEnterInMilliseconds = 40000;
        private int _timeRemainingInSeconds;
        private int _giveMeTimeToExitInMilliseconds = 20000;
        private bool _giveMeTimeToExit;
		private CommandEventArgs _alarmComandEventArgs;
        private List<SecuritySensor> _sensors = new List<SecuritySensor>();
        private List<SecurityActuator> _lockState = new List<SecurityActuator>();
        private List<SecurityActuator> _alarmHornState = new List<SecurityActuator>();
        private bool _alarmSounding;

		public MasterControlPanel(IMqtt client)
		{
			_client = client;
			_giveMeTimeToEnterTimer = new Timer(1000);
			_giveMeTimeToEnterTimer.Enabled = false;
            _giveMeTimeToEnterTimer.Elapsed += _giveMeTimeToEnterTimer_Elapsed;

            _giveMeTimeToExitTimer = new Timer(1000);
            _giveMeTimeToExitTimer.Enabled = false;
            _giveMeTimeToExitTimer.Elapsed += _giveMeTimeToExitTimer_Elapsed;

            _turnOffDoorbellIndicatorTimer = new Timer(3000);
            _turnOffDoorbellIndicatorTimer.Enabled = false;
            _turnOffDoorbellIndicatorTimer.Elapsed += _turnOffDoorbellIndicatorTimer_Elapsed;

            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "front", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "side", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "back", Sensor = "door", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "firstfloor", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "firstfloor", Sensor = "motion", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "masterbedroom", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom1", Sensor = "window", SensorValue = "closed" });
            _sensors.Add(new SecuritySensor { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom2", Sensor = "window", SensorValue = "closed" });

            _lockState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "front", Actuator = "lock", ActuatorValue = "unlocked" });
            _lockState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "back", Actuator = "lock", ActuatorValue = "unlocked" });
            _lockState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "externaldoor", LocationCode = "side", Actuator = "lock", ActuatorValue = "unlocked" });

            _alarmHornState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "masterbedroom", Actuator = "burglar", ActuatorValue = "off" });
            _alarmHornState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom1", Actuator = "burglar", ActuatorValue = "off" });
            _alarmHornState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "bedroom2", Actuator = "burglar", ActuatorValue = "off" });
            _alarmHornState.Add(new SecurityActuator { HouseCode = "house1", DeviceCode = "alarmpanel", LocationCode = "firstfloor", Actuator = "burglar", ActuatorValue = "off" });
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
                            context.Clients.updateCommand(args);

                            if (UnLockDoor(args))
                                DisarmAlarm(args);
                        }
                        if (args.Command.Equals("doorbell"))
                        {
                            _turnOffDoorbellIndicatorTimer.Enabled = true;
                            context.Clients.updateCommand(args);
                        }
                        if (args.Command.Equals("door"))
                        {
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("window"))
                        {
                            context.Clients.updateCommand(args);
                            
                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("motion"))
                        {
                            context.Clients.updateCommand(args);
                            
                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("lock"))
                        {
                            context.Clients.updateCommand(args);

                            ProcessLockStateChange(args);
                        }
                        break;
                    case "alarmpanel":
                        if (args.Command.Equals("alarmstate"))
                        {
                            context.Clients.updateCommand(args);

                            SetAlarmState(args);
                        }
                        if (args.Command.Equals("setalarmstate"))
                        {
                            context.Clients.updateCommand(args);
                        }
                        if (args.Command.Equals("emergency"))
                        {
                            context.Clients.updateCommand(args);

                            SoundBurglarAlarm(args);
                        }
                        if (args.Command.Equals("code"))
                        {
                            context.Clients.updateCommand(args);

                            DisarmAlarm(args);
                        }
                        if (args.Command.Equals("door"))
                        {
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("window"))
                        {
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("motion"))
                        {
                            context.Clients.updateCommand(args);

                            ProcessSensorStateChange(args);
                        }
                        if (args.Command.Equals("burglar"))
                        {
                            context.Clients.updateCommand(args);
                        }
                        break;
                }
            }
            else
                return false;
            return true;
        }

        public void PublishState()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeSecurityHub>();

            CommandEventArgs args = null;

            // Send the current Sensor state
            foreach (SecuritySensor sensor in _sensors)
            {
                args = new CommandEventArgs
                {
                    HouseCode = sensor.HouseCode,
                    DeviceCode = sensor.DeviceCode,
                    LocationCode = sensor.LocationCode,
                    Command = sensor.Sensor,
                    CommandValue = sensor.SensorValue,
                };

                context.Clients.updateCommand(args);
            }

            // Send the current state of the alarm
            args = new CommandEventArgs
            { 
                HouseCode = "house1", 
                DeviceCode = "alarmpanel",
                LocationCode = "masterbedroom",
                Command = "setalarmstate", 
                CommandValue = GetAlarmState(_currentState) };
            context.Clients.updateCommand(args);

            // Send the door lock state
            foreach (SecurityActuator actuator in _lockState)
            {
                args = new CommandEventArgs
                {
                    HouseCode = actuator.HouseCode,
                    DeviceCode = actuator.DeviceCode,
                    LocationCode = actuator.LocationCode,
                    Command = actuator.Actuator,
                    CommandValue = actuator.ActuatorValue,
                };

                context.Clients.updateCommand(args);

            }

            // TODO send the state of the Alarm Horns
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
                    else if (_currentState == AlarmState.Away)
                    {
                        _giveMeTimeToExit = true;
                        _timeRemainingInSeconds = _giveMeTimeToExitInMilliseconds / 1000;
                        _giveMeTimeToExitTimer.Enabled = true;
                    }
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
				_giveMeTimeToEnterTimer.Enabled = false;
				_currentState = AlarmState.Off;
                SendAlarmStateChange(args.HouseCode, _currentState);
				SilenceBurglarAlarm();
                _giveMeTimeToExitTimer.Enabled = false;
                _giveMeTimeToExit = false;
                disarmed = true;
			}
            return disarmed;
		}

        private void ProcessLockStateChange(CommandEventArgs args)
        {
            if (args == null || args.CommandValue == null || !(args.CommandValue.Equals("locked") || args.CommandValue.Equals("unlocked")))
                return;

            // Set the new state of the sensors
            var doorLock = _lockState.Where(s => s.HouseCode == args.HouseCode
                && s.DeviceCode == args.DeviceCode
                && s.LocationCode == args.LocationCode && s.Actuator == args.Command).FirstOrDefault();
            if (doorLock != null)
            {
                doorLock.ActuatorValue = args.CommandValue;
            }

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

            if (args.CommandValue.Equals("opened"))
            {
                if (_currentState == AlarmState.Sleep)
			    {
				    SoundBurglarAlarm(args);
			    }

			    if (_currentState == AlarmState.Away && _giveMeTimeToEnterTimer.Enabled == false && _giveMeTimeToExit==false)
			    {
				    _alarmComandEventArgs = args;
				    // Start a timer that allows the user to get in the door and disarm the alarm before it fires
                    _timeRemainingInSeconds = _giveMeTimeToEnterInMilliseconds / 1000;
                    _giveMeTimeToEnterTimer.Interval = 1000;
				    _giveMeTimeToEnterTimer.Enabled = true;
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

        void _giveMeTimeToEnterTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
            _timeRemainingInSeconds--;
            if (_timeRemainingInSeconds < 1)
            {
                _giveMeTimeToEnterTimer.Enabled = false;
                SoundBurglarAlarm(_alarmComandEventArgs);
            }

            // Update the browsers with the timer remaining value
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeSecurityHub>();
            context.Clients.updateEntryTimeRemaining(_timeRemainingInSeconds);

		}

        void _turnOffDoorbellIndicatorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _turnOffDoorbellIndicatorTimer.Enabled = false;
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeSecurityHub>();
            CommandEventArgs args = new CommandEventArgs("house1", "externaldoor", "front", "doorbell", "off");
            context.Clients.updateCommand(args);

            args = new CommandEventArgs("house1", "externaldoor", "back", "doorbell", "off"); 
            context.Clients.updateCommand(args);

            args = new CommandEventArgs("house1", "externaldoor", "side", "doorbell", "off");
            context.Clients.updateCommand(args);
        }

        void _giveMeTimeToExitTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timeRemainingInSeconds--;
            if (_timeRemainingInSeconds < 1)
            {
                _giveMeTimeToExitTimer.Enabled = false;
                _giveMeTimeToExit = false;
                LockAllDoors();
            }

            // Update the browsers with the timer remaining value
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<HomeSecurityHub>();
            context.Clients.updateEntryTimeRemaining(_timeRemainingInSeconds);

        }

        private void ArmSecuritySystem()
        {
            _giveMeTimeToExit = false;
        }

		private void SoundBurglarAlarm(CommandEventArgs args)
		{
            _alarmSounding = true;
            CommandEventArgs newArgs = null;
            if (args.DeviceCode.Equals("externaldoor"))
            {
                // Map all external door breakins to the first floor alarm panel
                newArgs = new CommandEventArgs
                {
                    HouseCode = args.HouseCode,
                    DeviceCode = "alarmpanel",
                    LocationCode = "firstfloor",
                    Command = "burglar",
                    CommandValue = "on"
                };
            }
            else
            {
                newArgs = new CommandEventArgs
                {
                    HouseCode = args.HouseCode,
                    DeviceCode = args.DeviceCode,
                    LocationCode = args.LocationCode,
                    Command = args.Command,
                    CommandValue = args.CommandValue
                };
            }

			// Use the args to determine where to send the alarm burglar trigger command
            string topic = string.Format("/{0}/{1}/{2}/{3}", newArgs.HouseCode, newArgs.DeviceCode, newArgs.LocationCode, "burglar");
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