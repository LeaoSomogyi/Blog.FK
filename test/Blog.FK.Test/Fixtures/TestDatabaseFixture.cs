using Blog.FK.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;

namespace Blog.FK.Test.Fixtures
{
    public class TestDatabaseFixture : IDisposable
    {
        #region "  Properties  "

        /// <summary>
        /// TestServer created by Startup.cs
        /// </summary>
        public TestServer Server { get; private set; }

        #endregion

        #region "  Constructor  "

        public TestDatabaseFixture()
        {
            var builder = Program.CreateWebHostBuilder(new string[0])
                .UseStartup<TestStartup>();

            Server = new TestServer(builder);
        }

        #endregion

        #region "  IDisposable  "

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
