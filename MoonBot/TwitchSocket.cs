using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;

namespace MoonBot
{
    public class TwitchSocket
    {
        public ClientWebSocket ClientWebSocket;
        public TwitchSocket(ClientWebSocket webSocket)
        {
            this.ClientWebSocket = webSocket;
        }

        public TwitchSocket()
        {
        }

        public async Task<ClientWebSocket> WebSocketConnectAsync()
        {
            bool connected = false;
            string _json = "{\"type\": \"PING\"}";

            var _url = new Uri("wss://pubsub-edge.twitch.tv");

            ClientWebSocket _ClientWebSocket = new ClientWebSocket();
            try
            {
                await _ClientWebSocket.ConnectAsync(_url, CancellationToken.None);
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(DateTime.Now.ToString("dd-MM-yyyy") + " : " + ex.Message);
                Console.WriteLine(sb);
            }

            byte[] _byteArray = Encoding.UTF8.GetBytes(_json);
            var _buffer = new ArraySegment<Byte>(_byteArray, 0, _byteArray.Length);
            await _ClientWebSocket.SendAsync(_buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            var _bufferReception = WebSocket.CreateClientBuffer(512, 512);
            await _ClientWebSocket.ReceiveAsync(_bufferReception, CancellationToken.None);


            string jsonStr = Encoding.UTF8.GetString(_bufferReception.Array);
            if (jsonStr.Contains("PONG"))
            {
                connected = true;
            }
            return _ClientWebSocket;
        }

        public async Task<string> SendPingAsync(ClientWebSocket clientwebsocket)
        {
            string jsonPing = "{\"type\": \"PING\"}";
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonPing);
            ArraySegment<Byte> buffer = new ArraySegment<Byte>(byteArray, 0, byteArray.Length);
            await clientwebsocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            ArraySegment<Byte> bufferReception = WebSocket.CreateClientBuffer(512, 512);
            await clientwebsocket.ReceiveAsync(bufferReception, CancellationToken.None);

            string result = Encoding.UTF8.GetString(bufferReception.Array);

            return result;

        }

        public async void BitsSubscribe(ClientWebSocket clientwebsocket)
        {
            string jsonBits = @"
                                {
    
                                    'type': 'LISTEN',
                                    'nonce': 'bits listen event',
                                    'data': {
                                                 'topics': ['channel-bits-events-v1.44322889'],
                                                 'auth_token': 'cfabdegwdoklmawdzdo98xt2fo512y'
                                            }
                                }";

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonBits);
            ArraySegment<Byte> buffer = new ArraySegment<Byte>(byteArray, 0, byteArray.Length);
            await clientwebsocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            ArraySegment<Byte> bufferReception = WebSocket.CreateClientBuffer(512, 512);
            await clientwebsocket.ReceiveAsync(bufferReception, CancellationToken.None);

            string result = Encoding.UTF8.GetString(bufferReception.Array);

        }
    }
}
