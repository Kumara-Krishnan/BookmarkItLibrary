using BookmarkItLibrary.Data.Handler;
using BookmarkItLibrary.Data.Handler.Contract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.DB;
using UWPUtilities.Adapter.DB.Contract;
using UWPUtilities.Adapter.Net;
using UWPUtilities.Adapter.Net.Contract;

namespace BookmarkItLibrary.DI
{
    public sealed class DIServiceProvider
    {
        public static DIServiceProvider Instance { get { return DIServiceProviderSingleton.Instance; } }

        private readonly IServiceProvider ServiceProvider;

        private DIServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IDBAdapter, SQLiteDBAdapter>();
            serviceCollection.AddSingleton<INetAdapter, NetAdapter>();

            serviceCollection.AddSingleton<IDBHandler, DBHandler>();
            serviceCollection.AddSingleton<INetHandler, NetHandler>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }

        private class DIServiceProviderSingleton
        {
            internal static readonly DIServiceProvider Instance = new DIServiceProvider();

            static DIServiceProviderSingleton() { }
        }
    }
}
