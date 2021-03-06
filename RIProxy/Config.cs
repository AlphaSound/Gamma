﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Gamma.Proxy
{
    public static class Config
    {
        public static readonly string ServerAddress;

        public static readonly int ServerPort;

        public static readonly string ProxyAddress;

        public static readonly int ProxyPort;

        static Config()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddXmlFile("RIProxy.config")
                .Build();
            ServerAddress = configuration["ServerAddress"];
            ServerPort = int.Parse(configuration["ServerPort"]);
            ProxyAddress = configuration["ProxyAddress"];
            ProxyPort = int.Parse(configuration["ProxyPort"]);
        }
    }
}
