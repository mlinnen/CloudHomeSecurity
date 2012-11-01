using MqttLib;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeSecurity.Web.Hubs
{
    public class HomeSecurityHub:Hub
    {
        private string _host = "168.62.188.28";
        private int _port = 1883;
        private static IMqtt _client;
        private string _clientId = "noc";

        public HomeSecurityHub()
        {
        }

        public void Send(string message)
        {
            Clients.addMessage(message);
        }
        public void SendConnectedMQTTClients(int count)
        {
            Clients.updateConnectedMQTTClients(count);
        }

        public void ConnectToBroker(string ip, int port, string clientId)
        {
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

            _host = ip;
            _port = port;
            _clientId = clientId;

            _client = MqttClientFactory.CreateClient("tcp://" + _host + ":" + _port.ToString(), _clientId);

            _client.Connect();
            _client.Subscribe("house1/#", QoS.AtLeastOnce);
            _client.Subscribe("$SYS/#", QoS.AtLeastOnce);
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
                if (e.Topic.Contains("/ping"))
                {
                    // TODO send a pingresp back to the original sender
                }
                if (e.Topic.Contains("$SYS/broker/clients/active"))
                {
                    int count = 0;
                    if (int.TryParse(e.Payload, out count))
                        SendConnectedMQTTClients(count);
                }
            }
            return true;
        }

    }
}