using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApiAsync.Models;

namespace WebApiAsync
{
    public class Program
    {
        public static int Delay = 60; 
        
        public static List<User> Users = new List<User>();
        public static List<Session> Sessions = new List<Session>();

        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {webBuilder.UseStartup<Startup>();});
    }
}
