
using System;
using System.Threading.Tasks;
using BookmarkItLibrary.Data.Handler.Contract;
using BookmarkItLibrary.DI;
using BookmarkItLibrary.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Security.Authentication.Web;

namespace BookmarkItLibraryTests
{
    [TestClass]
    public class AuthenticationTests
    {
        [TestMethod]
        public async Task GetRequestTokenAsync()
        {
            var netHandler = BookmarkItLibrary.DI.DIServiceProvider.Instance.GetService<INetHandler>();
            var requestToken = await netHandler.GetRequestTokenAsync();
            Assert.IsTrue(requestToken.StartsWith("code="));
        }


    }
}
