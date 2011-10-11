using System;
using System.Net;

namespace PI.WebGarten
{
    using PI.WebGarten.Pipeline;

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


        /// <summary>
        /// Gets the <see cref="HttpFilterPipeline"/> instance to register filters.
        /// </summary>
        public HttpFilterPipeline Pipeline
        {
            get {
                return _handler.Pipeline;
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
                if (_listener.IsListening) {
                    _listener.Close();
                }
            }
        }

    }
}
