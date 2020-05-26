using FamilyTreeXML.Infrastructure;
using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FamilyTreeXML.App
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            string connectionString = configuration["ConnectionString"];
            ConsoleAPI api = new ConsoleAPI(new FamilyTreeService(connectionString));

            api.Run();
        }
    }
}