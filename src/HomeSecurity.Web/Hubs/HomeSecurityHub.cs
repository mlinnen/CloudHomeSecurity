using Microsoft.AspNet.SignalR.Hubs;
using MqttLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
    public class HomeSecurityHub:Hub
    {
        private static IMqtt _client;
		private static MasterControlPanel _securitySystem;
        private object _myLock = new object();

        public HomeSecurityHub()
        {
			ConnectToBroker();
            if (_securitySystem==null)
			    _securitySystem = new MasterControlPanel(_client);
        }

		public bool ConnectedToMQTTBroker
		{
			get
			{
				if (_client == null)
					return false;
				else
					return true;
			}
		}

        public void Send(string message)
        {
            Clients.All.addMessage(message);
        }

        public void SendConnectedMQTTClients(int count)
        {
            Clients.All.updateConnectedMQTTClients(count);
        }

        public void PublishMessage(string topic, string msg)
        {
            _client.Publish(topic, new MqttPayload(msg), QoS.BestEfforts, false);
        }

        public void PublishCurrentState()
        {
            _securitySystem.PublishState();
        }

		public void ConnectToBroker()
		{
			if (_client == null)
			{
				string ip = ConfigurationManager.AppSettings["MQTTBrokerIp"];
				string noc = ConfigurationManager.AppSettings["MQTTClientId"];
				ConnectToBroker(ip, 1883, noc);
			}

            // Since someone new connected to the broker, go ahead and make sure the state is updated

		}

        private void ConnectToBroker(string ip, int port, string clientId)
        {
			if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(clientId) || port == 0)
				return;
            if (_client != null)
            {
                _client.PublishArrived -= new PublishArrivedDelegate(_client_PublishArrived);
                _client.Connected -= new ConnectionDelegate(_client_Connected);
                _client.ConnectionLost -= new ConnectionDelegate(_client_ConnectionLost);
                _client.Published -= new CompleteDelegate(_client_Published);
                _client.Subscribed -= new CompleteDelegate(_client_Subscribed);
                _client.Unsubscribed -= new CompleteDelegate(_client_Unsubscribed);
                if (_client.IsConnected)
                {
                    // TODO this might be async
                    _client.Disconnect();
                }
            }

			_client = MqttClientFactory.CreateClient("tcp://" + ip + ":" + port.ToString(), clientId);

            _client.Connect();

			// Subscribe to the house code in config
			string houseCode = ConfigurationManager.AppSettings["HouseCode"];

            _client.Subscribe("/" + houseCode + "/#", QoS.BestEfforts);
            _client.Subscribe("$SYS/broker/clients/#", QoS.BestEfforts);
            _client.PublishArrived += new PublishArrivedDelegate(_client_PublishArrived);
            _client.Connected += new ConnectionDelegate(_client_Connected);
            _client.ConnectionLost += new ConnectionDelegate(_client_ConnectionLost);
            _client.Published += new CompleteDelegate(_client_Published);
            _client.Subscribed += new CompleteDelegate(_client_Subscribed);
            _client.Unsubscribed += new CompleteDelegate(_client_Unsubscribed);

            

        }

        void _client_Unsubscribed(object sender, CompleteArgs e)
        {
        }

        void _client_Subscribed(object sender, CompleteArgs e)
        {
        }

        void _client_Published(object sender, CompleteArgs e)
        {
        }

        void _client_ConnectionLost(object sender, EventArgs e)
        {
        }

        void _client_Connected(object sender, EventArgs e)
        {
        }

        bool _client_PublishArrived(object sender, PublishArrivedArgs e)
        {
            if (e!=null && !string.IsNullOrEmpty(e.Topic))
            {
                // Is this a ping request
                if (e.Topic.EndsWith("/ping"))
                {
                    // Route the ping response back to the originator
                    _client.Publish(e.Topic.Replace("/ping", "/pingresp"), new MqttPayload(e.Payload), QoS.BestEfforts, false);
                }

                // Look for changes in the MQTT Client connections and send it to the browser
                if (e.Topic.Contains("$SYS/broker/clients/active"))
                {
                    int count = 0;
                    if (int.TryParse(e.Payload, out count))
                        SendConnectedMQTTClients(count);
                }

                CommandEventArgs args = CommandEventArgs.BuildCommandArgs(e.Topic, e.Payload);
                if (args != null)
                {
                    // Only allow 1 thread to update the state of the security system at one time
                    lock (_myLock)
                    {
                        _securitySystem.ProcessCommand(args);
                    }
                }

            }
            return true;
        }

	}
}