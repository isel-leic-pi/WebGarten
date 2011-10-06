using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PI.WebGarten.MethodBasedCommands;

namespace PI.WebGarten
{
    public class HttpListenerBasedHost
    {
        private readonly string _baseAddress;
        private readonly Handler _handler;
        private readonly HttpListener _listener;

        public HttpListenerBasedHost(string baseAddress)
        {
            _baseAddress = baseAddress;
            _handler = new Handler(_baseAddress);
            _listener = new HttpListener();
            _listener.Prefixes.Add(_baseAddress);
        }

        public void Add(params ICommand[] cmd)
        {
            foreach (var command in cmd)
            {
                _handler.Add(command);    
            }
            
        }

        public void OpenAndWaitForever()
        {
            try
            {
                _listener.Start();
                Console.WriteLine("Listener started");
                while (true)
                {
                    var ctx = _listener.GetContext();
                    _handler.Handle(ctx);
                }
            }
            finally
            {
                if(_listener.IsListening) _listener.Close();
            }
        }

    }
}
