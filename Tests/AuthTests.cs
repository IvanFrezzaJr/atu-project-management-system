using Xunit;
using ProjectManagementSystem;

namespace ProjectManagementSystem.Tests
{
    public class AuthTests
    {
        [Fact]
        public void TestUserAuthentication()
        {
            var auth = new Authentication();
            string username = "admin";
            string password = "12345";
            bool expected = true;

            bool result = auth.Authenticate(username, password);

            Assert.Equal(expected, result);
        }
    }
}
