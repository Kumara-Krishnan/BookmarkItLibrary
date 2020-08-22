using BookmarkItCommonLibrary.Data.Handler.Contract;
using BookmarkItCommonLibrary.DI;
using BookmarkItCommonLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Adapter.DB;
using UWPUtilities.Util;

namespace BookmarkItLibrary
{
    public sealed class ServiceManager
    {
        public static ServiceManager Instance { get { return ServiceManagerSingleton.Instance; } }

        private ServiceManager() { }

        public bool Initialize()
        {
            bool isInitialized = false;
            try
            {
                var dbHandler = CommonDIServiceProvider.Instance.GetService<IDBHandler>();
                dbHandler.Initialize(CommonConstants.DBFileName, FileSystemUtil.GetApplicationFolder(ApplicationFolderType.Local).Path);
                isInitialized = true;
            }
            catch { }
            return isInitialized;
        }

        private class ServiceManagerSingleton
        {
            static ServiceManagerSingleton() { }

            internal static readonly ServiceManager Instance = new ServiceManager();
        }
    }
}