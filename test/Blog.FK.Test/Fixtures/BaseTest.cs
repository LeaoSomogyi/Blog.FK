using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace Blog.FK.Test.Fixtures
{
    [Collection("DatabaseCollection")]
    public class BaseTest
    {
        #region "  Properties  "

        /// <summary>
        /// TestServer created by Startup.cs, only used to retrieve DI services
        /// </summary>
        public TestServer Server { get; private set; }

        #endregion

        #region "  Constructor  "

        public BaseTest(TestDatabaseFixture testDatabaseFixture)
        {
            Server = testDatabaseFixture.Server;
        }

        #endregion
    }
}
