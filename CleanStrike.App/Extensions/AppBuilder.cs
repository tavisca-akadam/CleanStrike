using CleanStrike.Interfaces;
using CleanStrike.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanStrike.App.Extensions
{
    internal static class AppBuilder
    {
        internal static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddTransient<GameController>();
                    services.AddSingleton<IAction, GameAction>();
                    services.AddSingleton<IGame, Game>();
                    services.AddTransient<CarromBoard>();
                });
        }
    }
}
