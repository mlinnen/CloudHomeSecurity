using MqttLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
	public class SecuritySystem
	{
		private AlarmState _currentState = AlarmState.Off;
		private int _secretCode = 01;
		private readonly IMqtt _client;
		private Timer _delayAlarm;
		private int _delayInMilliseconds = 10000;
		private CommandEventArgs _currentEventArgs;

		public SecuritySystem(IMqtt client)
		{
			_client = client;
			_delayAlarm = new Timer(_delayInMilliseconds);
			_delayAlarm.Enabled = false;
			_delayAlarm.Elapsed += _delayAlarm_Elapsed;
		}

		public void SetAlarmState(AlarmState newState)
		{
			if (newState == _currentState)
				return;

			if (_currentState == AlarmState.Off)
			{
				_currentState = newState;
			}
		}

		public bool DisarmAlarm(int code)
		{
			bool disarmed = false;

			if (code == _secretCode)
			{
				_delayAlarm.Enabled = false;
				_currentState = AlarmState.Off;
				SilenceBurglarAlarm();
				disarmed = true;
			}
			
			return disarmed;
		}

		public void ProcessSensorStateChange(CommandEventArgs args)
		{
			if (args == null)
				return;
			if (!args.CommandValue.Equals("opened"))
				return;

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

		void _delayAlarm_Elapsed(object sender, ElapsedEventArgs e)
		{
			_delayAlarm.Enabled = false;
			SoundBurglarAlarm(_currentEventArgs);

		}

		private void SoundBurglarAlarm(CommandEventArgs args)
		{
			// Use the args to determine where to send the alarm burglar trigger command
			string topic = string.Format("{0}/{1}/{2}/{3}", args.HouseCode, args.DeviceCode, args.LocationCode, "burglar");
			_client.Publish(topic, new MqttPayload("on"), QoS.BestEfforts, false);
		}

		private void SilenceBurglarAlarm()
		{
			_client.Publish("house1/alarmcontrol/masterbedroom/burglar", new MqttPayload("off"), QoS.BestEfforts, false);
			_client.Publish("house1/alarmcontrol/bedroom1/burglar", new MqttPayload("off"), QoS.BestEfforts, false);
			_client.Publish("house1/alarmcontrol/bedroom2/burglar", new MqttPayload("off"), QoS.BestEfforts, false);
			_client.Publish("house1/alarmcontrol/firstfloor/burglar", new MqttPayload("off"), QoS.BestEfforts, false);
		}
	}
}