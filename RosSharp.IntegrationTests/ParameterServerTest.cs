﻿using System;
using System.Reactive.Linq;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RosSharp.Master;
using RosSharp.Parameter;

namespace RosSharp.IntegrationTests
{
    [TestClass]
    public class ParameterServerTest
    {
        private MasterServer _masterServer;
        private static TimeSpan TestTimeout = TimeSpan.FromSeconds(3);

        [TestInitialize]
        public void Initialize()
        {
            Ros.MasterUri = new Uri("http://localhost:11311/");
            Ros.HostName = "localhost";
            Ros.TopicTimeout = 3000;
            Ros.XmlRpcTimeout = 3000;

            _masterServer = new MasterServer(11311);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _masterServer.Dispose();
            Ros.Dispose();
        }

        [TestMethod]
        public void IntParameter()
        {
            var node = Ros.CreateNode("test");
            
            var param = node.CreateParameterAsync<int>("test_param").Result;

            param.Subscribe(x => Console.WriteLine("param = {0}", x));

            for(int i=0;i<10;i++)
            {
                param.Value = i;
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

        }
    }
}
