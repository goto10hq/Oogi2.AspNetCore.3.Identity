namespace Oogi2.AspNetCore3.Identity.Tests.Entities
{    
    public class TestIdentityRole : IdentityRole
    {
        public TestIdentityRole() : base("oogi2/role", "oogi2")
        {
        }
    }
}